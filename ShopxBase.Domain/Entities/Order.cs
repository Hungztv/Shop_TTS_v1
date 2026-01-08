namespace ShopxBase.Domain.Entities;

using ShopxBase.Domain.Enums;

/// <summary>
/// Order entity
/// </summary>
public class Order : BaseEntity
{
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string Notes { get; set; }

    public User User { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public Order()
    {
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        PaymentStatus = PaymentStatus.Pending;
    }

    public Order(int userId, string shippingAddress) : this()
    {
        UserId = userId;
        ShippingAddress = shippingAddress;
    }
}

/// <summary>
/// Order Item entity (chi tiết sản phẩm trong đơn hàng)
/// </summary>
public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public Order Order { get; set; }
    public Product Product { get; set; }

    public OrderItem()
    {
    }

    public OrderItem(int orderId, int productId, int quantity, decimal unitPrice)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
    }
}
