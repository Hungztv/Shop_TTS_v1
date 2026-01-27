using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.Compare;
using ShopxBase.Application.Features.Compare.Commands.AddToCompare;
using ShopxBase.Application.Features.Compare.Commands.RemoveFromCompare;
using ShopxBase.Application.Features.Compare.Commands.ClearCompare;
using ShopxBase.Application.Features.Compare.Queries.GetUserCompare;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Controller for managing product comparison lists
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompareController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompareController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get current user's compare list with full product details
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CompareSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CompareSummaryDto>> GetCompareList()
    {
        var query = new GetUserCompareQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Add a product to compare list (max 5 items)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CompareItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CompareItemDto>> AddToCompare([FromBody] AddToCompareDto dto)
    {
        var command = new AddToCompareCommand
        {
            ProductId = dto.ProductId
        };

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCompareList), result);
    }

    /// <summary>
    /// Remove an item from compare list by compare ID
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromCompare(int id)
    {
        var command = new RemoveFromCompareCommand
        {
            CompareId = id
        };

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Clear all items from compare list
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ClearCompareList()
    {
        var command = new ClearCompareCommand();
        await _mediator.Send(command);
        return NoContent();
    }
}
