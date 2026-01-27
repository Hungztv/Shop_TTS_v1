using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Wishlist.Commands.RemoveFromWishlist;

public class RemoveFromWishlistCommandHandler : IRequestHandler<RemoveFromWishlistCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RemoveFromWishlistCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Get wishlist item
        var wishlistItem = await _unitOfWork.Wishlists.GetByIdAsync(request.WishlistId);
        if (wishlistItem == null)
            throw WishlistNotFoundException.NotFound(request.WishlistId);

        // 3. SECURITY: Validate ownership
        if (wishlistItem.UserId != userId)
            throw WishlistUnauthorizedException.UnauthorizedAccess();

        // 4. Delete item
        await _unitOfWork.Wishlists.DeletePermanentlyAsync(request.WishlistId);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
