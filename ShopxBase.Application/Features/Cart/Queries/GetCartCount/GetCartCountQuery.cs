using MediatR;

namespace ShopxBase.Application.Features.Cart.Queries.GetCartCount;

/// <summary>
/// Query to get the count of items in the user's cart
/// </summary>
public class GetCartCountQuery : IRequest<int>
{
    /// <summary>
    /// User ID whose cart count to retrieve
    /// </summary>
    public string UserId { get; set; }

    public GetCartCountQuery()
    {
    }

    public GetCartCountQuery(string userId)
    {
        UserId = userId;
    }
}
