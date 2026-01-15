using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Domain.Entities
{
    public class Coupon : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        // Date Range
        public DateTime DateStart { get; set; }
        public DateTime DateExpired { get; set; }

        // Discount Configuration
        [Range(0, 100000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; }

        public bool IsPercent { get; set; }

        // Quantity & Usage
        [Required]
        public int Quantity { get; set; }

        public int UsedCount { get; set; }

        // Minimum Order
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumOrderValue { get; set; }

        // Status
        public int Status { get; set; }

        // Navigation Properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Business Methods
        public bool IsValid()
        {
            var now = DateTime.Now;
            return Status == 1
                && Quantity > UsedCount
                && now >= DateStart
                && now <= DateExpired;
        }

        public bool CanApply(decimal orderValue)
        {
            return IsValid() && orderValue >= MinimumOrderValue;
        }

        public decimal CalculateDiscount(decimal orderValue)
        {
            if (!CanApply(orderValue))
                return 0;

            if (IsPercent)
            {
                return orderValue * (DiscountValue / 100);
            }

            return DiscountValue;
        }

        public void Use()
        {
            if (!IsValid())
                throw InvalidCouponException.Inactive(Code);

            UsedCount++;
        }

        public bool IsExpired()
        {
            return DateTime.Now > DateExpired;
        }

        public bool IsOutOfStock()
        {
            return UsedCount >= Quantity;
        }
    }
}