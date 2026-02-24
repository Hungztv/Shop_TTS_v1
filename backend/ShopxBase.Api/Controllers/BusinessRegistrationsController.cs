using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.BusinessRegistration;
using ShopxBase.Application.Features.BusinessRegistrations.Commands.ApproveBusinessRegistration;
using ShopxBase.Application.Features.BusinessRegistrations.Commands.CreateBusinessRegistration;
using ShopxBase.Application.Features.BusinessRegistrations.Commands.RejectBusinessRegistration;
using ShopxBase.Application.Features.BusinessRegistrations.Queries.AdminListBusinessRegistrations;
using ShopxBase.Application.Features.BusinessRegistrations.Queries.GetMyBusinessRegistration;
using ShopxBase.Domain.Enums;

namespace ShopxBase.Api.Controllers;

[Authorize]
public class BusinessRegistrationsController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBusinessRegistrationCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Gửi đăng ký kinh doanh thành công");
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyRegistration()
    {
        var result = await Mediator.Send(new GetMyBusinessRegistrationQuery());
        if (result == null)
            return Error("Chưa có đăng ký", 404);

        return Success(result, "Lấy đăng ký kinh doanh thành công");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminList([FromQuery] BusinessRegistrationStatus? status)
    {
        var result = await Mediator.Send(new AdminListBusinessRegistrationsQuery { Status = status });
        return Success(result, "Lấy danh sách đăng ký kinh doanh thành công");
    }

    [HttpPatch("{id:int}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await Mediator.Send(new ApproveBusinessRegistrationCommand { Id = id });
        return Success(result, "Duyệt đăng ký kinh doanh thành công");
    }

    [HttpPatch("{id:int}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reject(int id, [FromBody] ReviewBusinessRegistrationDto dto)
    {
        var result = await Mediator.Send(new RejectBusinessRegistrationCommand
        {
            Id = id,
            RejectReason = dto.RejectReason
        });
        return Success(result, "Từ chối đăng ký kinh doanh thành công");
    }
}
