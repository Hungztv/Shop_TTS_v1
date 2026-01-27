using MediatR;
using ShopxBase.Application.DTOs.Rating;

namespace ShopxBase.Application.Features.Ratings.Queries.GetProductRatings;

/// <summary>
/// Query to get paginated ratings for a product
/// </summary>
public class GetProductRatingsQuery : IRequest<RatingPagedDto>
{
    public int ProductId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
