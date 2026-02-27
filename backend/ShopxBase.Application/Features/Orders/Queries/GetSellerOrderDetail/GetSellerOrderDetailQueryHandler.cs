using MediatR;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Orders.Queries.GetSellerOrderDetail;

public class GetSellerOrderDetailQueryHandler : IRequestHandler<GetSellerOrderDetailQuery, SellerOrderDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetSellerOrderDetailQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<SellerOrderDetailDto> Handle(GetSellerOrderDetailQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw UnauthorizedUserException.UserIdNotFound();

        var shop = await _unitOfWork.Shops.FirstOrDefaultAsync(s => s.OwnerUserId == userId && !s.IsDeleted);
        if (shop == null)
            throw new ShopNotFoundException("Bạn chưa có shop");

        // Get order with details
        var order = await _unitOfWork.OrderRepository.GetWithDetailsAsync(request.OrderId);
        if (order == null)
            throw OrderNotFoundException.WithMessage("Không tìm thấy đơn hàng");

        // Filter to only this shop's items
        var shopDetails = order.OrderDetails.Where(d => d.ShopId == shop.Id).ToList();
        if (!shopDetails.Any())
            throw OrderNotFoundException.WithMessage("Đơn hàng không chứa sản phẩm của shop bạn");

        return new SellerOrderDetailDto
        {
            OrderId = order.Id,
            OrderCode = order.OrderCode,
            CustomerName = order.Name,
            CustomerPhone = order.PhoneNumber,
            Address = order.Address,
            Email = order.Email,
            Note = order.Note,
            Status = order.Status,
            StatusText = order.GetStatusText(),
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            CreatedAt = order.CreatedAt,
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
    }
}
