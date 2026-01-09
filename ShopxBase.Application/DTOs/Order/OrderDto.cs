using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.DTOs.Order;

/// <summary>
/// Order DTO for read operations
/// </summary>
public class OrderDto
{
    public int Id { get; set; }
    public string OrderCode { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Note { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    public string CouponCode { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public int Status { get; set; }
    public string StatusText { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
}

/// <summary>
/// OrderDetail DTO
/// </summary>
public class OrderDetailDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}

/// <summary>
/// Create Order DTO
/// </summary>
public class CreateOrderDto
{
    [Required(ErrorMessage = "Tên người nhận là bắt buộc")]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
    [MaxLength(500)]
    public string Address { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(1000)]
    public string Note { get; set; }

    [MaxLength(50)]
    public string CouponCode { get; set; }

    [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc")]
    [MaxLength(50)]
    public string PaymentMethod { get; set; }

    public string UserId { get; set; }

    [Required(ErrorMessage = "Đơn hàng phải có ít nhất 1 sản phẩm")]
    [MinLength(1, ErrorMessage = "Đơn hàng phải có ít nhất 1 sản phẩm")]
    public List<CreateOrderDetailDto> OrderDetails { get; set; }

    // Note: Subtotal, ShippingCost, DiscountAmount, Total will be calculated by server
}

/// <summary>
/// Create OrderDetail DTO
/// </summary>
public class CreateOrderDetailDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; }
}

/// <summary>
/// Update Order Status DTO
/// </summary>
public class UpdateOrderStatusDto
{
    public int Id { get; set; }
    public int Status { get; set; }
}
