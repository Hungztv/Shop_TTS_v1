using MediatR;
using ShopxBase.Application.DTOs.Wishlist;

namespace ShopxBase.Application.Features.Wishlist.Commands.AddToWishlist;

/// <summary>
/// Command to add a product to user's wishlist
/// </summary>
public class AddToWishlistCommand : IRequest<WishlistDto>
{
    public int ProductId { get; set; }

    // UserId will be set by the handler from ICurrentUserService
    public string UserId { get; set; } = string.Empty;
}
