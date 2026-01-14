using MediatR;
using ShopxBase.Application.DTOs.Order;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQuery : IRequest<PaginationResponse<OrderDto>>
{
    // TODO: Add properties
}
