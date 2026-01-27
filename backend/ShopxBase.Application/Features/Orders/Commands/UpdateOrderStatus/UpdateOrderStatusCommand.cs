using MediatR;
using ShopxBase.Application.DTOs.Order;

namespace ShopxBase.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IRequest<OrderDto>
{
    public int OrderId { get; set; }
    public int NewStatus { get; set; }

    // Optional: Who is making this change (for audit)
    public string? UpdatedBy { get; set; }
}
