using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Cart.Commands.ClearCart;

/// <summary>
/// Handler for ClearCartCommand
/// </summary>
public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ClearCartCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        // SECURITY: Validate that user is authenticated
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        // Get the authenticated user's ID from token (ignore request.UserId for security)
        var userId = _currentUserService.UserId;

        // 1. Get all cart items for authenticated user
        var cartItems = await _unitOfWork.Carts.FindAsync(c => c.UserId == userId);

        // 2. Delete all items
        foreach (var item in cartItems)
        {
            await _unitOfWork.Carts.DeletePermanentlyAsync(item.Id);
        }

        // 3. Save changes
        await _unitOfWork.SaveChangesAsync();

        // 4. Return true
        return true;
    }
}
