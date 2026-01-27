using MediatR;
using ShopxBase.Application.DTOs.Order;

namespace ShopxBase.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public int OrderId { get; set; }

    public GetOrderByIdQuery(int orderId)
    {
        OrderId = orderId;
    }
}
