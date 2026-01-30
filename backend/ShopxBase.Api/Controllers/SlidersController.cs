using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.Slider;
using ShopxBase.Application.Features.Sliders.Commands.CreateSlider;
using ShopxBase.Application.Features.Sliders.Commands.DeleteSlider;
using ShopxBase.Application.Features.Sliders.Commands.ReorderSliders;
using ShopxBase.Application.Features.Sliders.Commands.UpdateSlider;
using ShopxBase.Application.Features.Sliders.Queries.GetActiveSliders;
using ShopxBase.Application.Features.Sliders.Queries.GetAllSliders;
using ShopxBase.Application.Features.Sliders.Queries.GetSliderById;

namespace ShopxBase.Api.Controllers;

public class SlidersController : BaseApiController
{
    /// <summary>
    /// Get all sliders (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllSlidersQuery());
        return Success(result, "Lấy danh sách slider thành công");
    }

    /// <summary>
    /// Get active sliders (Public)
    /// </summary>
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive()
    {
        var result = await Mediator.Send(new GetActiveSlidersQuery());
        return Success(result, "Lấy danh sách slider đang hoạt động thành công");
    }

    /// <summary>
    /// Get slider by ID (Admin only)
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetSliderByIdQuery(id));
        if (result == null)
            return Error("Không tìm thấy slider", 404);

        return Success(result, "Lấy thông tin slider thành công");
    }

    /// <summary>
    /// Create new slider (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSliderCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo slider thành công");
    }

    /// <summary>
    /// Update slider (Admin only)
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSliderCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        if (result == null)
            return Error("Không tìm thấy slider", 404);

        return Success(result, "Cập nhật slider thành công");
    }

    /// <summary>
    /// Delete slider (Admin only)
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteSliderCommand(id));
        if (!result)
            return Error("Không tìm thấy slider", 404);

        return Success(result, "Xóa slider thành công");
    }

    /// <summary>
    /// Reorder sliders (Admin only)
    /// </summary>
    [HttpPut("reorder")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reorder([FromBody] ReorderSlidersCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result)
            return Error("Không thể sắp xếp lại slider");

        return Success(result, "Sắp xếp slider thành công");
    }
}
