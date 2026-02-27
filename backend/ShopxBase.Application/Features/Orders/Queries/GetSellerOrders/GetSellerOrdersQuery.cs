using MediatR;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Orders.Queries.GetSellerOrders;

public class GetSellerOrdersQuery : IRequest<PaginationResponse<SellerOrderListItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? Status { get; set; }
    public string? Search { get; set; }
}
