using MediatR;
using AutoMapper;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Application.DTOs.Common;
using System.Linq.Expressions;

namespace ShopxBase.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PaginationResponse<OrderListItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<OrderListItemDto>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        // Build filter - use separate variables for EF Core compatibility
        string? userId = request.UserId;
        int? status = request.Status;

        Expression<Func<Order, bool>> predicate = o =>
            (string.IsNullOrEmpty(userId) || o.UserId == userId) &&
            (!status.HasValue || o.Status == status.Value);

        // Get paginated orders
        var (orders, totalCount) = await _unitOfWork.OrderRepository
            .GetFilteredAsync(predicate, request.PageNumber, request.PageSize);

        // Map to list item DTOs (lighter than full OrderDto)
        var orderDtos = orders.Select(o => new OrderListItemDto
        {
            Id = o.Id,
            OrderCode = o.OrderCode,
            Name = o.Name,
            PhoneNumber = o.PhoneNumber,
            Total = o.Total,
            Status = o.Status,
            StatusText = o.GetStatusText(),
            PaymentMethod = o.PaymentMethod,
            PaymentStatus = o.PaymentStatus,
            CreatedAt = o.CreatedAt,
            TotalItems = o.OrderDetails?.Count ?? 0
        }).ToList();

        // Return paginated response
        return new PaginationResponse<OrderListItemDto>
        {
            Items = orderDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
