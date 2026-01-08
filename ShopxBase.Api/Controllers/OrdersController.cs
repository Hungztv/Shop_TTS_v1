namespace ShopxBase.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs;
using ShopxBase.Application.Interfaces;

/// <summary>
/// Orders API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Get order by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    /// <summary>
    /// Get orders by user id
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return Ok(orders);
    }

    /// <summary>
    /// Create new order
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
    {
        var order = await _orderService.CreateOrderAsync(createOrderDto);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Update order status
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
    {
        updateOrderStatusDto.OrderId = id;
        var order = await _orderService.UpdateOrderStatusAsync(updateOrderStatusDto);
        return Ok(order);
    }

    /// <summary>
    /// Cancel order
    /// </summary>
    [HttpDelete("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _orderService.CancelOrderAsync(id);
        return result ? Ok(new { message = "Order cancelled successfully" }) : NotFound();
    }

    /// <summary>
    /// Get order count by user
    /// </summary>
    [HttpGet("user/{userId}/count")]
    public async Task<IActionResult> GetOrderCount(int userId)
    {
        var count = await _orderService.GetOrderCountByUserAsync(userId);
        return Ok(new { userId, orderCount = count });
    }
}
