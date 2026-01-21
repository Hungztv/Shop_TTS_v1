using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Users.Commands.UpdateUserProfile;
using ShopxBase.Application.Features.Users.Commands.DeleteUser;
using ShopxBase.Application.Features.Users.Queries.GetUsers;
using ShopxBase.Application.Features.Users.Queries.GetUserProfile;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Users API Controller - CQRS Pattern
/// </summary>
[Authorize]
public class UsersController : BaseApiController
{
    /// <summary>
    /// Lấy danh sách users với pagination và filters
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    /// <summary>
    /// Lấy profile user theo ID
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetProfile(string userId)
    {
        var result = await Mediator.Send(new GetUserProfileQuery { UserId = userId });
        return Success(result);
    }

    /// <summary>
    /// Cập nhật profile user
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(string id, [FromBody] UpdateUserProfileCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật profile thành công");
    }

    /// <summary>
    /// Xóa user
    /// </summary>
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(string userId)
    {
        await Mediator.Send(new DeleteUserCommand { UserId = userId });
        return Success(true, "Xóa người dùng thành công");
    }
}
