using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Wishlist;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Wishlist.Queries.GetUserWishlist;

public class GetUserWishlistQueryHandler : IRequestHandler<GetUserWishlistQuery, WishlistSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetUserWishlistQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<WishlistSummaryDto> Handle(GetUserWishlistQuery request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Get all wishlist items for user
        var wishlistItems = await _unitOfWork.Wishlists.FindAsync(w => w.UserId == userId);

        // 3. Build response DTOs with product details
        var items = new List<WishlistDto>();

        foreach (var item in wishlistItems)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

            // Skip if product is deleted
            if (product == null || product.IsDeleted)
                continue;

            items.Add(new WishlistDto
            {
                Id = item.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSlug = product.Slug,
                ProductImage = product.Image,
                ProductPrice = product.Price,
                ProductCapitalPrice = product.CapitalPrice,
                ProductQuantity = product.Quantity,
                IsInStock = product.Quantity > 0,
                BrandName = product.Brand?.Name ?? "",
                CategoryName = product.Category?.Name ?? "",
                CreatedAt = item.CreatedAt
            });
        }

        // 4. Return summary
        return new WishlistSummaryDto
        {
            Items = items,
            TotalItems = items.Count
        };
    }
}
