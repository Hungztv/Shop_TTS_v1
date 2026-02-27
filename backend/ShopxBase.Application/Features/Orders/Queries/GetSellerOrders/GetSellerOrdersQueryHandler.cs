using MediatR;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Application.DTOs.Common;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Orders.Queries.GetSellerOrders;

public class GetSellerOrdersQueryHandler : IRequestHandler<GetSellerOrdersQuery, PaginationResponse<SellerOrderListItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetSellerOrdersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<PaginationResponse<SellerOrderListItemDto>> Handle(GetSellerOrdersQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        var shop = await _unitOfWork.Shops.FirstOrDefaultAsync(s => s.OwnerUserId == userId && !s.IsDeleted);
        if (shop == null)
            throw new ShopNotFoundException("Bạn chưa có shop");

        var (orders, totalCount) = await _unitOfWork.OrderRepository.GetOrdersByShopAsync(
            shop.Id, request.Status, request.Search, request.PageNumber, request.PageSize);

        var items = orders.Select(o =>
        {
            // Filter OrderDetails to only show items from seller's shop
            var shopDetails = o.OrderDetails.Where(d => d.ShopId == shop.Id).ToList();

            return new SellerOrderListItemDto
            {
                OrderId = o.Id,
                OrderCode = o.OrderCode,
                CustomerName = o.Name,
                CustomerPhone = o.PhoneNumber,
                Address = o.Address,
                Status = o.Status,
                StatusText = o.GetStatusText(),
                PaymentMethod = o.PaymentMethod,
                PaymentStatus = o.PaymentStatus,
                CreatedAt = o.CreatedAt,
                ItemCount = shopDetails.Sum(d => d.Quantity),
                ShopSubtotal = shopDetails.Sum(d => d.Price * d.Quantity),
                ShopOrderDetails = shopDetails.Select(d => new OrderDetailDto
                {
                    Id = d.Id,
                    ProductName = d.ProductName,
                    ProductImage = d.ProductImage,
                    Price = d.Price,
                    Quantity = d.Quantity,
                    Total = d.Price * d.Quantity,
                    ShopId = d.ShopId,
                    ShopName = d.ShopName
                }).ToList()
            };
        }).ToList();

        return new PaginationResponse<SellerOrderListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
