using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Auth.Commands.Login;
using ShopxBase.Application.Features.Auth.Commands.Register;
using ShopxBase.Application.Features.Auth.Commands.RefreshToken;
using ShopxBase.Application.Features.Auth.Queries.GetCurrentUser;
using System.Security.Claims;

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
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Đăng ký thành công");
    }

    /// <summary>
    /// Đăng nhập
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Đăng nhập thành công");
    }

    /// <summary>
    /// Làm mới access token
    /// </summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Làm mới token thành công");
    }

    /// <summary>
    /// Lấy thông tin user hiện tại
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await Mediator.Send(new GetCurrentUserQuery { UserId = userId });
        return Success(result);
    }

    /// <summary>
    /// Đăng xuất (xóa refresh token)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        // Optional: Implement logout logic to invalidate refresh token
        // For now, client should just delete the tokens
        return Success(true, "Đăng xuất thành công");
    }
}
