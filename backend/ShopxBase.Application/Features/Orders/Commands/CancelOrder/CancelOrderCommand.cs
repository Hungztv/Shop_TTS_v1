using MediatR;

namespace ShopxBase.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommand : IRequest<bool>
{
    public int OrderId { get; set; }
}
