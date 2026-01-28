using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ShopxBase.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected IActionResult Success<T>(T data, string message = "Success")
    {
        return Ok(new
        {
            success = true,
            message,
            data
        });
    }

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
