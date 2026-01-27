using MediatR;

namespace ShopxBase.Application.Features.Cart.Commands.ClearCart;

/// <summary>
/// Command to clear all items from the user's cart
/// </summary>
public class ClearCartCommand : IRequest<bool>
{
    /// <summary>
    /// User ID whose cart to clear
    /// </summary>
    public string UserId { get; set; }
}
