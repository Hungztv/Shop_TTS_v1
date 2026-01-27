using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.Rating;
using ShopxBase.Application.Features.Ratings.Commands.CreateRating;
using ShopxBase.Application.Features.Ratings.Commands.UpdateRating;
using ShopxBase.Application.Features.Ratings.Commands.DeleteRating;
using ShopxBase.Application.Features.Ratings.Queries.GetProductRatings;
using ShopxBase.Application.Features.Ratings.Queries.GetRatingStats;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Controller for managing product ratings
/// </summary>
[ApiController]
[Route("api")]
public class RatingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get paginated ratings for a product
    /// </summary>
    [HttpGet("products/{productId:int}/ratings")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RatingPagedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RatingPagedDto>> GetProductRatings(
        int productId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetProductRatingsQuery
        {
            ProductId = productId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get rating statistics for a product (distribution)
    /// </summary>
    [HttpGet("products/{productId:int}/ratings/stats")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RatingStatsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RatingStatsDto>> GetRatingStats(int productId)
    {
        var query = new GetRatingStatsQuery
        {
            ProductId = productId
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a new rating for a product
    /// </summary>
    [HttpPost("products/{productId:int}/ratings")]
    [Authorize]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RatingDto>> CreateRating(int productId, [FromBody] CreateRatingDto dto)
    {
        var command = new CreateRatingCommand
        {
            ProductId = productId,
            Star = dto.Star,
            Comment = dto.Comment,
            Name = dto.Name,
            Email = dto.Email
        };

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductRatings), new { productId = result.ProductId }, result);
    }

    /// <summary>
    /// Update an existing rating
    /// </summary>
    [HttpPut("ratings/{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(RatingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RatingDto>> UpdateRating(int id, [FromBody] UpdateRatingDto dto)
    {
        var command = new UpdateRatingCommand
        {
            RatingId = id,
            Star = dto.Star,
            Comment = dto.Comment
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Delete a rating
    /// </summary>
    [HttpDelete("ratings/{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRating(int id)
    {
        var command = new DeleteRatingCommand
        {
            RatingId = id
        };

        await _mediator.Send(command);
        return NoContent();
    }
}
