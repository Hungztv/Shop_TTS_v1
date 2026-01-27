using MediatR;

namespace ShopxBase.Application.Features.Wishlist.Commands.RemoveFromWishlist;

/// <summary>
/// Command to remove a product from user's wishlist
/// </summary>
public class RemoveFromWishlistCommand : IRequest<bool>
{
    public int WishlistId { get; set; }

    // UserId for ownership validation (set from ICurrentUserService)
    public string UserId { get; set; } = string.Empty;
}
