using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Shops.Commands.UpdateShop;
using ShopxBase.Application.Features.Shops.Queries.GetMyShop;

namespace ShopxBase.Api.Controllers;

[Authorize]
public class ShopsController : BaseApiController
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyShop()
    {
        var result = await Mediator.Send(new GetMyShopQuery());
        if (result == null)
            return Error("Chưa có shop", 404);

        return Success(result, "Lấy thông tin shop thành công");
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShopCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật shop thành công");
    }
}
