using MediatR;
using ShopxBase.Application.DTOs.Cart;

namespace ShopxBase.Application.Features.Cart.Commands.AddToCart;


public class AddToCartCommand : IRequest<CartDto>
{

    public string UserId { get; set; }

    public int ProductId { get; set; }


    public int Quantity { get; set; }
}
