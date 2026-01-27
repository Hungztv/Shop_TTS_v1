using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Compare;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Application.Features.Compare.Queries.GetUserCompare;

public class GetUserCompareQueryHandler : IRequestHandler<GetUserCompareQuery, CompareSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetUserCompareQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<CompareSummaryDto> Handle(GetUserCompareQuery request, CancellationToken cancellationToken)
    {
        // 1. SECURITY: Get user ID from token
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw UnauthorizedUserException.UserIdNotFound();

        var userId = _currentUserService.UserId;

        // 2. Get all compare items for user
        var compareItems = await _unitOfWork.CompareProducts.FindAsync(c => c.UserId == userId);

        // 3. Build response DTOs with full product details
        var items = new List<CompareItemDto>();

        foreach (var item in compareItems)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

            // Skip if product is deleted
            if (product == null || product.IsDeleted)
                continue;

            items.Add(new CompareItemDto
            {
                Id = item.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSlug = product.Slug,
                ProductDescription = product.Description,
                ProductImage = product.Image,
                ProductPrice = product.Price,
                ProductCapitalPrice = product.CapitalPrice,
                ProductQuantity = product.Quantity,
                ProductSoldOut = product.SoldOut,
                IsInStock = product.Quantity > 0,
                AverageScore = product.AverageScore,
                RatingCount = product.RatingCount,
                BrandId = product.BrandId,
                BrandName = product.Brand?.Name ?? "",
                BrandLogo = product.Brand?.Logo ?? "",
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "",
                CreatedAt = item.CreatedAt
            });
        }

        // 4. Return summary
        return new CompareSummaryDto
        {
            Items = items,
            TotalItems = items.Count,
            MaxItems = 5
        };
    }
}
