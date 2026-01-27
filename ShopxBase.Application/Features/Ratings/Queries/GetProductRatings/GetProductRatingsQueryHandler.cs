using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Rating;

namespace ShopxBase.Application.Features.Ratings.Queries.GetProductRatings;

public class GetProductRatingsQueryHandler : IRequestHandler<GetProductRatingsQuery, RatingPagedDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductRatingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RatingPagedDto> Handle(GetProductRatingsQuery request, CancellationToken cancellationToken)
    {
        // 1. Validate product exists
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
        if (product == null || product.IsDeleted)
            throw ProductNotFoundException.WithMessage("Product not found");

        // 2. Get all approved ratings for this product
        var allRatings = await _unitOfWork.Ratings.FindAsync(
            r => r.ProductId == request.ProductId && !r.IsDeleted && r.IsApproved);

        var ratingsList = allRatings.OrderByDescending(r => r.CreatedAt).ToList();
        var totalItems = ratingsList.Count;

        // 3. Apply pagination
        var pagedRatings = ratingsList
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // 4. Build response DTOs
        var items = new List<RatingDto>();
        foreach (var rating in pagedRatings)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(rating.UserId);

            items.Add(new RatingDto
            {
                Id = rating.Id,
                ProductId = rating.ProductId,
                ProductName = product.Name,
                UserId = rating.UserId,
                UserName = user?.FullName ?? rating.Name,
                Star = rating.Star,
                StarDisplay = rating.GetStarDisplay(),
                Comment = rating.Comment,
                Name = rating.Name,
                Email = rating.Email,
                IsVerifiedPurchase = rating.IsVerifiedPurchase,
                IsApproved = rating.IsApproved,
                CreatedAt = rating.CreatedAt
            });
        }

        // 5. Return paginated result
        return new RatingPagedDto
        {
            Items = items,
            TotalItems = totalItems,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
