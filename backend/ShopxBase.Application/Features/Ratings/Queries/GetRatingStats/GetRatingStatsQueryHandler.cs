using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Application.DTOs.Rating;

namespace ShopxBase.Application.Features.Ratings.Queries.GetRatingStats;

public class GetRatingStatsQueryHandler : IRequestHandler<GetRatingStatsQuery, RatingStatsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRatingStatsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RatingStatsDto> Handle(GetRatingStatsQuery request, CancellationToken cancellationToken)
    {
        // 1. Validate product exists
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
        if (product == null || product.IsDeleted)
            throw ProductNotFoundException.WithMessage("Product not found");

        // 2. Get all approved ratings for this product
        var ratings = await _unitOfWork.Ratings.FindAsync(
            r => r.ProductId == request.ProductId && !r.IsDeleted && r.IsApproved);

        var ratingsList = ratings.ToList();
        var totalRatings = ratingsList.Count;

        // 3. Calculate distribution
        var fiveStarCount = ratingsList.Count(r => r.Star == 5);
        var fourStarCount = ratingsList.Count(r => r.Star == 4);
        var threeStarCount = ratingsList.Count(r => r.Star == 3);
        var twoStarCount = ratingsList.Count(r => r.Star == 2);
        var oneStarCount = ratingsList.Count(r => r.Star == 1);

        // 4. Calculate average (use cached value from product for performance)
        var averageScore = totalRatings > 0
            ? Math.Round((decimal)ratingsList.Average(r => r.Star), 2)
            : 0;

        // 5. Build response
        return new RatingStatsDto
        {
            ProductId = request.ProductId,
            AverageScore = averageScore,
            TotalRatings = totalRatings,
            FiveStarCount = fiveStarCount,
            FourStarCount = fourStarCount,
            ThreeStarCount = threeStarCount,
            TwoStarCount = twoStarCount,
            OneStarCount = oneStarCount
        };
    }
}
