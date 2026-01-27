using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Wishlist.Commands.ClearWishlist;

public class ClearWishlistCommandHandler : IRequestHandler<ClearWishlistCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ClearWishlistCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ClearWishlistCommand request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Get all wishlist items for user
        var wishlistItems = await _unitOfWork.Wishlists.FindAsync(w => w.UserId == userId);

        // 3. Delete all items
        foreach (var item in wishlistItems)
        {
            await _unitOfWork.Wishlists.DeletePermanentlyAsync(item.Id);
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
