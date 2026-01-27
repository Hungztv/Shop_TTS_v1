using MediatR;
using ShopxBase.Application.DTOs.Rating;

namespace ShopxBase.Application.Features.Ratings.Queries.GetRatingStats;

/// <summary>
/// Query to get rating statistics for a product
/// </summary>
public class GetRatingStatsQuery : IRequest<RatingStatsDto>
{
    public int ProductId { get; set; }
}
