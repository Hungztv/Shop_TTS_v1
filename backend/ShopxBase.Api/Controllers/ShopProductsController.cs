using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.ShopProducts.Commands.CreateShopProduct;
using ShopxBase.Application.Features.ShopProducts.Commands.DeleteShopProduct;
using ShopxBase.Application.Features.ShopProducts.Commands.UpdateShopProduct;
using ShopxBase.Application.Features.ShopProducts.Queries.GetShopProducts;

namespace ShopxBase.Api.Controllers;


[Route("api/shops/me/products")]
[Authorize(Roles = "Seller,Admin")]
public class ShopProductsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetMyProducts([FromQuery] GetShopProductsQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result, "Lấy sản phẩm của shop thành công");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShopProductCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo sản phẩm cho shop thành công");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateShopProductCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật sản phẩm cho shop thành công");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteShopProductCommand { Id = id });
        return Success(result, "Xóa sản phẩm khỏi shop thành công");
    }
}
