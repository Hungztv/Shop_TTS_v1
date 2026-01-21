using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Categories.Commands.CreateCategory;
using ShopxBase.Application.Features.Categories.Commands.UpdateCategory;
using ShopxBase.Application.Features.Categories.Commands.DeleteCategory;
using ShopxBase.Application.Features.Categories.Commands.DeleteCategoryPermanently;
using ShopxBase.Application.Features.Categories.Queries.GetCategories;
using ShopxBase.Application.Features.Categories.Queries.GetCategoryById;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Categories API Controller - CQRS Pattern
/// </summary>
public class CategoriesController : BaseApiController
{
    /// <summary>
    /// Lấy danh sách categories với pagination
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    /// <summary>
    /// Lấy category theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetCategoryByIdQuery { Id = id });
        return Success(result);
    }

    /// <summary>
    /// Tạo category mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo danh mục thành công");
    }

    /// <summary>
    /// Cập nhật category
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật danh mục thành công");
    }

    /// <summary>
    /// Xóa category
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCategoryCommand { Id = id });
        return Success(true, "Xóa danh mục thành công (soft delete)");
    }

    /// <summary>
    /// Xóa vĩnh viễn category khỏi database
    /// </summary>
    [HttpDelete("{id}/permanent")]
    public async Task<IActionResult> DeletePermanently(int id)
    {
        await Mediator.Send(new DeleteCategoryPermanentlyCommand { Id = id });
        return Success(true, "Đã xóa vĩnh viễn danh mục khỏi database");
    }
}
