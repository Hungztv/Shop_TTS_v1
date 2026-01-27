using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.Wishlist;
using ShopxBase.Application.Features.Wishlist.Commands.AddToWishlist;
using ShopxBase.Application.Features.Wishlist.Commands.RemoveFromWishlist;
using ShopxBase.Application.Features.Wishlist.Commands.ClearWishlist;
using ShopxBase.Application.Features.Wishlist.Queries.GetUserWishlist;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Controller for managing user wishlists
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WishlistController : ControllerBase
{
    private readonly IMediator _mediator;

    public WishlistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get current user's wishlist
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(WishlistSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<WishlistSummaryDto>> GetWishlist()
    {
        var query = new GetUserWishlistQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Add a product to wishlist
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(WishlistDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<WishlistDto>> AddToWishlist([FromBody] AddToWishlistDto dto)
    {
        var command = new AddToWishlistCommand
        {
            ProductId = dto.ProductId
        };

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetWishlist), result);
    }

    /// <summary>
    /// Remove an item from wishlist by wishlist ID
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromWishlist(int id)
    {
        var command = new RemoveFromWishlistCommand
        {
            WishlistId = id
        };

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Clear all items from wishlist
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ClearWishlist()
    {
        var command = new ClearWishlistCommand();
        await _mediator.Send(command);
        return NoContent();
    }
}
