using MediatR;

namespace ShopxBase.Application.Features.Wishlist.Commands.ClearWishlist;

/// <summary>
/// Command to clear all items from user's wishlist
/// </summary>
public class ClearWishlistCommand : IRequest<bool>
{
    // UserId for ownership (set from ICurrentUserService)
    public string UserId { get; set; } = string.Empty;
}
