using MediatR;
using ShopxBase.Application.DTOs.Order;
using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<OrderDto>
{
    [Required(ErrorMessage = "User ID is required")]
    public string UserId { get; set; }

    /// Recipient name
    [Required(ErrorMessage = "Recipient name is required")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    [MaxLength(100, ErrorMessage = "Name must not exceed 100 characters")]
    public string Name { get; set; }
    /// Recipient phone number
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [MaxLength(20, ErrorMessage = "Phone must not exceed 20 characters")]
    public string PhoneNumber { get; set; }
    /// Delivery address
    [Required(ErrorMessage = "Address is required")]
    [MinLength(5, ErrorMessage = "Address must be at least 5 characters")]
    [MaxLength(500, ErrorMessage = "Address must not exceed 500 characters")]
    public string Address { get; set; }
    /// Email
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100, ErrorMessage = "Email must not exceed 100 characters")]
    public string Email { get; set; }
    /// Additional notes/comments (optional)
    [MaxLength(1000, ErrorMessage = "Notes must not exceed 1000 characters")]
    public string Note { get; set; }
    [Required(ErrorMessage = "Payment method is required")]
    [MaxLength(50, ErrorMessage = "Payment method must not exceed 50 characters")]
    public string PaymentMethod { get; set; }


    /// Coupon code (optional)

    [MaxLength(50, ErrorMessage = "Coupon code must not exceed 50 characters")]
    public string CouponCode { get; set; }


    /// Order line items (required - minimum 1 item)

    [Required(ErrorMessage = "Order must contain at least one item")]
    [MinLength(1, ErrorMessage = "Order must contain at least one item")]
    public List<CreateOrderDetailCommand> OrderDetails { get; set; } = new();
}


/// Order detail item in CreateOrderCommand

public class CreateOrderDetailCommand
{

    /// Product ID

    [Required(ErrorMessage = "Product ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid product ID")]
    public int ProductId { get; set; }


    /// Quantity ordered

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

}
