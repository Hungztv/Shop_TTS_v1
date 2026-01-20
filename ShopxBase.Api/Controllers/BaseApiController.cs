using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Base API Controller với MediatR support
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    /// <summary>
    /// Mediator instance để gửi commands/queries
    /// </summary>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    /// <summary>
    /// Trả về success response với data
    /// </summary>
    protected IActionResult Success<T>(T data, string message = "Success")
    {
        return Ok(new
        {
            success = true,
            message,
            data
        });
    }

    /// <summary>
    /// Trả về error response
    /// </summary>
    protected IActionResult Error(string message, int statusCode = 400)
    {
        return StatusCode(statusCode, new
        {
            success = false,
            message,
            errors = new[] { message }
        });
    }
}
