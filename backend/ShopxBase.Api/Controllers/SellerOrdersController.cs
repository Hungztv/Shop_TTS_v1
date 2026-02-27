using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Orders.Commands.UpdateSellerOrderStatus;
using ShopxBase.Application.Features.Orders.Queries.GetSellerOrders;
using ShopxBase.Application.Features.Orders.Queries.GetSellerOrderDetail;

namespace ShopxBase.Api.Controllers;

[Route("api/seller/orders")]
[Authorize(Roles = "Seller,Admin")]
public class SellerOrdersController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] GetSellerOrdersQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetSellerOrderDetailQuery { OrderId = id });
        return Success(result);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateSellerOrderStatusCommand command)
    {
        if (id != command.OrderId)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật trạng thái đơn hàng thành công");
    }
}
