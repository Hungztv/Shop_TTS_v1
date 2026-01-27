using MediatR;
using ShopxBase.Application.DTOs.Rating;

namespace ShopxBase.Application.Features.Ratings.Commands.CreateRating;

/// <summary>
/// Command to create a new rating for a product
/// </summary>
public class CreateRatingCommand : IRequest<RatingDto>
{
    public int ProductId { get; set; }
    public int Star { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // UserId will be set from ICurrentUserService
    public string UserId { get; set; } = string.Empty;
}
