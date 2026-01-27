using MediatR;
using ShopxBase.Application.DTOs.Wishlist;

namespace ShopxBase.Application.Features.Wishlist.Queries.GetUserWishlist;

/// <summary>
/// Query to get user's wishlist summary
/// </summary>
public class GetUserWishlistQuery : IRequest<WishlistSummaryDto>
{
    // UserId for ownership (set from ICurrentUserService)
    public string UserId { get; set; } = string.Empty;
}
