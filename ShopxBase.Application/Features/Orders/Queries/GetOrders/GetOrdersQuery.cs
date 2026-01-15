using MediatR;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQuery : IRequest<PaginationResponse<OrderListItemDto>>
{
    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Filters (optional)
    public string? UserId { get; set; }
    public int? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
