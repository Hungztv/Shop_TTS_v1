using MediatR;
using ShopxBase.Application.DTOs.Rating;

namespace ShopxBase.Application.Features.Ratings.Commands.UpdateRating;

/// <summary>
/// Command to update an existing rating
/// </summary>
public class UpdateRatingCommand : IRequest<RatingDto>
{
    public int RatingId { get; set; }
    public int Star { get; set; }
    public string Comment { get; set; } = string.Empty;

    // UserId for ownership validation (set from ICurrentUserService)
    public string UserId { get; set; } = string.Empty;
}
