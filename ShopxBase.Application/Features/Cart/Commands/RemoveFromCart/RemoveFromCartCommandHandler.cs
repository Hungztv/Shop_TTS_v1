using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Cart.Commands.RemoveFromCart;

/// <summary>
/// Handler for RemoveFromCartCommand
/// </summary>
public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RemoveFromCartCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        // SECURITY: Validate that user is authenticated
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        // Get the authenticated user's ID from token
        var userId = _currentUserService.UserId;

        // 1. Get cart item
        var cartItem = await _unitOfWork.Carts.GetByIdAsync(request.CartId);

        // 2. Validate cart item exists
        if (cartItem == null)
            throw CartNotFoundException.WithMessage("Giỏ hàng không tồn tại");

        // 3. SECURITY: Validate cart item belongs to current user (from token, not request)
        if (cartItem.UserId != userId)
            throw CartUnauthorizedException.UnauthorizedAccess();

        // 4. Delete cart item (permanent delete since cart items are not soft-deleted)
        await _unitOfWork.Carts.DeletePermanentlyAsync(request.CartId);

        // 5. Save changes
        await _unitOfWork.SaveChangesAsync();

        // 6. Return true
        return true;
    }
}
