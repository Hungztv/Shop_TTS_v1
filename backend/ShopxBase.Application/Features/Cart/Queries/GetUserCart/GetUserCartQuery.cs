using MediatR;
using ShopxBase.Application.DTOs.Cart;

namespace ShopxBase.Application.Features.Cart.Queries.GetUserCart;

/// <summary>
/// Query to get the user's cart with all items
/// </summary>
public class GetUserCartQuery : IRequest<CartSummaryDto>
{
    /// <summary>
    /// User ID whose cart to retrieve
    /// </summary>
    public string UserId { get; set; }

    public GetUserCartQuery()
    {
    }

    public GetUserCartQuery(string userId)
    {
        UserId = userId;
    }
}
