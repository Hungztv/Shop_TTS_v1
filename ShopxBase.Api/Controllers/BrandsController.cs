using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Brands.Commands.CreateBrand;
using ShopxBase.Application.Features.Brands.Commands.UpdateBrand;
using ShopxBase.Application.Features.Brands.Commands.DeleteBrand;
using ShopxBase.Application.Features.Brands.Commands.DeleteBrandPermanently;
using ShopxBase.Application.Features.Brands.Queries.GetBrands;
using ShopxBase.Application.Features.Brands.Queries.GetBrandById;

namespace ShopxBase.Api.Controllers;


/// Brands API Controller - CQRS Pattern

public class BrandsController : BaseApiController
{

    /// Lấy danh sách brands với pagination

    [HttpGet]
    public async Task<IActionResult> GetBrands([FromQuery] GetBrandsQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }


    /// Lấy brand theo ID

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetBrandByIdQuery { Id = id });
        return Success(result);
    }


    /// Tạo brand mới

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBrandCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo thương hiệu thành công");
    }


    /// Cập nhật brand

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật thương hiệu thành công");
    }


    /// Xóa brand

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteBrandCommand { Id = id });
        return Success(true, "Xóa thương hiệu thành công (soft delete)");
    }

    /// Xóa vĩnh viễn brand khỏi database
    [HttpDelete("{id}/permanent")]
    public async Task<IActionResult> DeletePermanently(int id)
    {
        await Mediator.Send(new DeleteBrandPermanentlyCommand { Id = id });
        return Success(true, "Đã xóa vĩnh viễn thương hiệu khỏi database");
    }
}
