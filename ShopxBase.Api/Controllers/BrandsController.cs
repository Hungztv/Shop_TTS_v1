using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Brands.Commands.CreateBrand;
using ShopxBase.Application.Features.Brands.Commands.UpdateBrand;
using ShopxBase.Application.Features.Brands.Commands.DeleteBrand;
using ShopxBase.Application.Features.Brands.Queries.GetBrands;
using ShopxBase.Application.Features.Brands.Queries.GetBrandById;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Brands API Controller - CQRS Pattern
/// </summary>
public class BrandsController : BaseApiController
{
    /// <summary>
    /// Lấy danh sách brands với pagination
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetBrands([FromQuery] GetBrandsQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    /// <summary>
    /// Lấy brand theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetBrandByIdQuery { Id = id });
        return Success(result);
    }

    /// <summary>
    /// Tạo brand mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBrandCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo thương hiệu thành công");
    }

    /// <summary>
    /// Cập nhật brand
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật thương hiệu thành công");
    }

    /// <summary>
    /// Xóa brand
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteBrandCommand { Id = id });
        return Success(true, "Xóa thương hiệu thành công");
    }
}
