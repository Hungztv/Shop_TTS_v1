using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Auth.Commands.Login;
using ShopxBase.Application.Features.Auth.Commands.Register;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Authentication API Controller
/// </summary>
public class AuthController : BaseApiController
{
    /// <summary>
    /// Đăng ký tài khoản mới
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Đăng ký thành công");
    }

    /// <summary>
    /// Đăng nhập
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Đăng nhập thành công");
    }
}
