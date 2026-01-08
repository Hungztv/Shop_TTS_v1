using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopxBase.Domain.Entities
{
    public class Order : BaseEntity
    {
        // Order Information
        [Required]
        public string OrderCode { get; set; }

        // Customer Information
        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public string Email { get; set; }
        public string Note { get; set; }

        // Pricing
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        // Coupon
        public string CouponCode { get; set; }
        public int? CouponId { get; set; }

        // Payment & Status
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public int Status { get; set; }

        // Foreign Keys
        public string UserId { get; set; }

        // Navigation Properties
        public virtual AppUser User { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        // Business Methods
        public string GetStatusText()
        {
            return Status switch
            {
                0 => "Chờ xử lý",
                1 => "Đã xác nhận",
                2 => "Đang giao hàng",
                3 => "Đã giao hàng",
                4 => "Đã hủy",
                _ => "Không xác định"
            };
        }

        public bool CanCancel()
        {
            return Status <= 1; // Chỉ hủy được khi chờ xử lý hoặc đã xác nhận
        }

        public void CalculateTotal()
        {
            Total = Subtotal + ShippingCost - DiscountAmount;
        }

        public bool IsPending()
        {
            return Status == 0;
        }

        public bool IsCompleted()
        {
            return Status == 3;
        }

        public bool IsCancelled()
        {
            return Status == 4;
        }
    }
}