namespace ShopxBase.Application.Interfaces;

using ShopxBase.Application.DTOs;

/// <summary>
/// Order service interface
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Get order by id
    /// </summary>
    Task<OrderDto> GetOrderByIdAsync(int id);

    /// <summary>
    /// Get all orders
    /// </summary>
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();

    /// <summary>
    /// Get orders by user id
    /// </summary>
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);

    /// <summary>
    /// Create new order
    /// </summary>
    Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);

    /// <summary>
    /// Update order status
    /// </summary>
    Task<OrderDto> UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto);

    /// <summary>
    /// Cancel order
    /// </summary>
    Task<bool> CancelOrderAsync(int orderId);

    /// <summary>
    /// Get order count by user
    /// </summary>
    Task<int> GetOrderCountByUserAsync(int userId);
}
