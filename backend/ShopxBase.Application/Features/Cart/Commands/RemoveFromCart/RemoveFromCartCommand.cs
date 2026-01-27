using MediatR;

namespace ShopxBase.Application.Features.Cart.Commands.RemoveFromCart;


public class RemoveFromCartCommand : IRequest<bool>
{

    public int CartId { get; set; }

    /// <summary>
    /// User ID for authorization
    /// </summary>
    public string UserId { get; set; }
}
