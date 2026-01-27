using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.Features.Orders.Commands.CreateOrder;
using ShopxBase.Application.Features.Orders.Commands.UpdateOrderStatus;
using ShopxBase.Application.Features.Orders.Commands.CancelOrder;
using ShopxBase.Application.Features.Orders.Queries.GetOrders;
using ShopxBase.Application.Features.Orders.Queries.GetOrderById;

namespace ShopxBase.Api.Controllers;


[Authorize]
public class OrdersController : BaseApiController
{

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("Không thể xác định người dùng");
    }


    private bool IsAdminOrSeller()
    {
        return User.IsInRole("Admin") || User.IsInRole("Seller");
    }


    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersQuery query)
    {
        // For customers, force filter by their own UserId
        if (!IsAdminOrSeller())
        {
            query.UserId = GetUserId();
        }

        var result = await Mediator.Send(query);
        return Success(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(new GetOrderByIdQuery(id));
        return Success(result);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        // CRITICAL: Override UserId from token for security
        command.UserId = GetUserId();

        var result = await Mediator.Send(command);
        return Success(result, "Tạo đơn hàng thành công");
    }


    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusCommand command)
    {
        if (id != command.OrderId)
            return BadRequest("ID không khớp");

        var result = await Mediator.Send(command);
        return Success(result, "Cập nhật trạng thái đơn hàng thành công");
    }


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
