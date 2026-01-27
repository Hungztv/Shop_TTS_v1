using MediatR;

namespace ShopxBase.Application.Features.Ratings.Commands.DeleteRating;

/// <summary>
/// Command to delete a rating
/// </summary>
public class DeleteRatingCommand : IRequest<bool>
{
    public int RatingId { get; set; }

    // UserId for ownership validation (set from ICurrentUserService)
    public string UserId { get; set; } = string.Empty;
}
