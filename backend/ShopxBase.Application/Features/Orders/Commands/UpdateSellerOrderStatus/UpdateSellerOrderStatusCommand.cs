using MediatR;

namespace ShopxBase.Application.Features.Orders.Commands.UpdateSellerOrderStatus;

public class UpdateSellerOrderStatusCommand : IRequest<bool>
{
    public int OrderId { get; set; }
    public int NewStatus { get; set; }
}
