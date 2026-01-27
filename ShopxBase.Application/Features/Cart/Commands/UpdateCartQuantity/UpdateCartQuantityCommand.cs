using MediatR;
using ShopxBase.Application.DTOs.Cart;

namespace ShopxBase.Application.Features.Cart.Commands.UpdateCartQuantity;


public class UpdateCartQuantityCommand : IRequest<CartDto>
{

    public int CartId { get; set; }

    public string UserId { get; set; }

    public int Quantity { get; set; }
}
