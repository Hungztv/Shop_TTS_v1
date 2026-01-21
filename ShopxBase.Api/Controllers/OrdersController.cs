using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Orders.Commands.CreateOrder;
using ShopxBase.Application.Features.Orders.Commands.UpdateOrderStatus;
using ShopxBase.Application.Features.Orders.Commands.CancelOrder;
using ShopxBase.Application.Features.Orders.Queries.GetOrders;
using ShopxBase.Application.Features.Orders.Queries.GetOrderById;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Orders API Controller - CQRS Pattern
/// </summary>
[Authorize]
public class OrdersController : BaseApiController
{
    /// <summary>
    /// Lấy danh sách orders với pagination và filters
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersQuery query)
    {
        var result = await Mediator.Send(query);
        return Success(result);
    }

    /// <summary>
    /// Lấy order theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetOrderByIdQuery(id));
        return Success(result);
    }

    /// <summary>
    /// Tạo order mới
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        var result = await Mediator.Send(command);
        return Success(result, "Tạo đơn hàng thành công");
    }

    /// <summary>
    /// Cập nhật trạng thái order
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusCommand command)
    {
        if (id != command.OrderId)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật trạng thái đơn hàng thành công");
    }

    /// <summary>
    /// Hủy order
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id, [FromBody] CancelOrderCommand command)
    {
        if (id != command.OrderId)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Hủy đơn hàng thành công");
    }
}
