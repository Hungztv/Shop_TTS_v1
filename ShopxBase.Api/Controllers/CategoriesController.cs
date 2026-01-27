using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Categories.Commands.CreateCategory;
using ShopxBase.Application.Features.Categories.Commands.UpdateCategory;
using ShopxBase.Application.Features.Categories.Commands.DeleteCategory;
using ShopxBase.Application.Features.Categories.Commands.DeleteCategoryPermanently;
using ShopxBase.Application.Features.Categories.Queries.GetCategories;
using ShopxBase.Application.Features.Categories.Queries.GetCategoryById;

namespace ShopxBase.Api.Controllers;


public class CategoriesController : BaseApiController
{

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }


    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetCategoryByIdQuery { Id = id });
        return Success(result);
    }


    [HttpPost]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo danh mục thành công");
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật danh mục thành công");
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCategoryCommand { Id = id });
        return Success(true, "Xóa danh mục thành công (soft delete)");
    }


    [HttpDelete("{id}/permanent")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePermanently(int id)
    {
        await Mediator.Send(new DeleteCategoryPermanentlyCommand { Id = id });
        return Success(true, "Đã xóa vĩnh viễn danh mục khỏi database");
    }
}
