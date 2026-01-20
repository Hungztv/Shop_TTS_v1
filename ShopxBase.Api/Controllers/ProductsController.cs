using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Products.Commands.CreateProduct;
using ShopxBase.Application.Features.Products.Commands.UpdateProduct;
using ShopxBase.Application.Features.Products.Commands.DeleteProduct;
using ShopxBase.Application.Features.Products.Queries.GetProducts;
using ShopxBase.Application.Features.Products.Queries.GetProductById;
using ShopxBase.Application.Features.Products.Queries.GetProductBySlug;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Products API Controller - CQRS Pattern
/// </summary>
public class ProductsController : BaseApiController
{
    /// <summary>
    /// Lấy danh sách products với pagination và filters
    /// </summary>
    /// <param name="query">Query parameters</param>
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    /// <summary>
    /// Lấy product theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetProductByIdQuery { Id = id });
        return Success(result);
    }

    /// <summary>
    /// Lấy product theo slug
    /// </summary>
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await Mediator.Send(new GetProductBySlugQuery { Slug = slug });
        return Success(result);
    }

    /// <summary>
    /// Tạo product mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo sản phẩm thành công");
    }

    /// <summary>
    /// Cập nhật product
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật sản phẩm thành công");
    }

    /// <summary>
    /// Xóa product
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProductCommand { Id = id });
        return Success(true, "Xóa sản phẩm thành công");
    }
}
