using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Shops.Commands.UpdateShop;
using ShopxBase.Application.Features.Shops.Queries.GetMyShop;
using ShopxBase.Application.Features.Shops.Queries.GetShopBySlug;
using ShopxBase.Application.Features.Shops.Queries.GetShopProducts;

namespace ShopxBase.Api.Controllers;

public class ShopsController : BaseApiController
{
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyShop()
    {
        var result = await Mediator.Send(new GetMyShopQuery());
        if (result == null)
            return Error("Chưa có shop", 404);

        return Success(result, "Lấy thông tin shop thành công");
    }

    [HttpPatch("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShopCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật shop thành công");
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await Mediator.Send(new GetShopBySlugQuery { Slug = slug });
        return Success(result);
    }

    [HttpGet("{shopId:int}/products")]
    [AllowAnonymous]
    public async Task<IActionResult> GetShopProducts(int shopId, [FromQuery] GetShopProductsPublicQuery query)
    {
        query.ShopId = shopId;
        var result = await Mediator.Send(query);
        return Success(result);
    }
}
