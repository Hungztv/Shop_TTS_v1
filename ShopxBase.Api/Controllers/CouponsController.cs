using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Coupons.Commands.CreateCoupon;
using ShopxBase.Application.Features.Coupons.Commands.UpdateCoupon;
using ShopxBase.Application.Features.Coupons.Commands.DeleteCoupon;
using ShopxBase.Application.Features.Coupons.Queries.GetCoupons;
using ShopxBase.Application.Features.Coupons.Queries.GetCouponByCode;
using ShopxBase.Application.Features.Coupons.Queries.ValidateCoupon;

namespace ShopxBase.Api.Controllers;


public class CouponsController : BaseApiController
{

    [HttpGet]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> GetCoupons([FromQuery] GetCouponsQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }


    [HttpGet("code/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCode(string code)
    {
        var result = await Mediator.Send(new GetCouponByCodeQuery { Code = code });
        return Success(result);
    }


    [HttpPost("validate")]
    [Authorize]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }


    [HttpPost]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> Create([FromBody] CreateCouponCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo mã giảm giá thành công");
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCouponCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật mã giảm giá thành công");
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCouponCommand { Id = id });
        return Success(true, "Xóa mã giảm giá thành công");
    }
}
