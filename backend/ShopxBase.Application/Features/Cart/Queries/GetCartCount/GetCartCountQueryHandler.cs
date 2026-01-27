using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Cart.Queries.GetCartCount;

/// <summary>
/// Handler for GetCartCountQuery
/// </summary>
public class GetCartCountQueryHandler : IRequestHandler<GetCartCountQuery, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetCartCountQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(GetCartCountQuery request, CancellationToken cancellationToken)
    {
        // SECURITY: Validate that user is authenticated
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        // Get the authenticated user's ID from token (ignore request.UserId for security)
        var userId = _currentUserService.UserId;

        // 1. Get all cart items for authenticated user
        var cartItems = await _unitOfWork.Carts.FindAsync(c => c.UserId == userId);

        // 2. Count items (only count items with valid products)
        var count = 0;
        foreach (var item in cartItems)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                count++;
            }
        }

        // 3. Return count
        return count;
    }
}
