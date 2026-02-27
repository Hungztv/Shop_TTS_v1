using ShopxBase.Application.DTOs.Order;

namespace ShopxBase.Application.DTOs.Order;

/// <summary>
/// DTO for seller's view of an order list item
/// Shows only info relevant to the seller's shop
/// </summary>
public class SellerOrderListItemDto
{
    public int OrderId { get; set; }
    public string OrderCode { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string Address { get; set; }
    public int Status { get; set; }
    public string StatusText { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    // Only counts items belonging to seller's shop
    public int ItemCount { get; set; }
    public decimal ShopSubtotal { get; set; }
    public List<OrderDetailDto> ShopOrderDetails { get; set; }
}

/// <summary>
/// DTO for seller's detailed view of an order
/// </summary>
public class SellerOrderDetailDto
{
    public int OrderId { get; set; }
    public string OrderCode { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Note { get; set; }
    public int Status { get; set; }
    public string StatusText { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemCount { get; set; }
    public decimal ShopSubtotal { get; set; }
    public List<OrderDetailDto> ShopOrderDetails { get; set; }
}
