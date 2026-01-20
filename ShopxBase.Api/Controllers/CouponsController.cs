using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Coupons.Commands.CreateCoupon;
using ShopxBase.Application.Features.Coupons.Commands.UpdateCoupon;
using ShopxBase.Application.Features.Coupons.Commands.DeleteCoupon;
using ShopxBase.Application.Features.Coupons.Queries.GetCoupons;
using ShopxBase.Application.Features.Coupons.Queries.GetCouponByCode;
using ShopxBase.Application.Features.Coupons.Queries.ValidateCoupon;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Coupons API Controller - CQRS Pattern
/// </summary>
public class CouponsController : BaseApiController
{
    /// <summary>
    /// Lấy danh sách coupons với pagination và filters
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCoupons([FromQuery] GetCouponsQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    /// <summary>
    /// Lấy coupon theo code
    /// </summary>
    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var result = await Mediator.Send(new GetCouponByCodeQuery { Code = code });
        return Success(result);
    }

    /// <summary>
    /// Validate coupon và tính discount
    /// </summary>
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    /// <summary>
    /// Tạo coupon mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCouponCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo mã giảm giá thành công");
    }

    /// <summary>
    /// Cập nhật coupon
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCouponCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật mã giảm giá thành công");
    }

    /// <summary>
    /// Xóa coupon
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCouponCommand { Id = id });
        return Success(true, "Xóa mã giảm giá thành công");
    }
}
