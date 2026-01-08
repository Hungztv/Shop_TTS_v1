namespace ShopxBase.Application.DTOs;

using ShopxBase.Domain.Enums;

/// <summary>
/// Order Data Transfer Object
/// </summary>
public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string Notes { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}

/// <summary>
/// Order Item Data Transfer Object
/// </summary>
public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

/// <summary>
/// Create Order Request DTO
/// </summary>
public class CreateOrderDto
{
    public int UserId { get; set; }
    public string ShippingAddress { get; set; }
    public string Notes { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}

/// <summary>
/// Create Order Item Request DTO
/// </summary>
public class CreateOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

/// <summary>
/// Update Order Status Request DTO
/// </summary>
public class UpdateOrderStatusDto
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}
