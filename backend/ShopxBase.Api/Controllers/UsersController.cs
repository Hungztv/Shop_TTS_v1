using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Users.Commands.UpdateUserProfile;
using ShopxBase.Application.Features.Users.Commands.DeleteUser;
using ShopxBase.Application.Features.Users.Queries.GetUsers;
using ShopxBase.Application.Features.Users.Queries.GetUserProfile;

namespace ShopxBase.Api.Controllers;


[Authorize]
public class UsersController : BaseApiController
{


    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("Không thể xác định người dùng");
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetUserId();
        var result = await Mediator.Send(new GetUserProfileQuery { UserId = userId });
        return Success(result);
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetProfile(string userId)
    {
        var currentUserId = GetUserId();
        var isAdmin = User.IsInRole("Admin");

        // Only allow access if admin or accessing own profile
        if (!isAdmin && currentUserId != userId)
            return Forbid();

        var result = await Mediator.Send(new GetUserProfileQuery { UserId = userId });
        return Success(result);
    }


    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileCommand command)
    {
        var userId = GetUserId();
        command.Id = userId; // Override with token user ID for security

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật profile thành công");
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(string id, [FromBody] UpdateUserProfileCommand command)
    {
        var currentUserId = GetUserId();
        var isAdmin = User.IsInRole("Admin");

        // Only allow access if admin or updating own profile
        if (!isAdmin && currentUserId != id)
            return Forbid();

        command.Id = id; // Ensure ID matches route

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật profile thành công");
    }


    [HttpDelete("{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string userId)
    {
        await Mediator.Send(new DeleteUserCommand { UserId = userId });
        return Success(true, "Xóa người dùng thành công");
    }
}
