using System.ComponentModel.DataAnnotations.Schema;

namespace ShopxBase.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        // Order Information
        public string OrderCode { get; set; }

        // Product Information
        public string ProductName { get; set; }
        public string ProductImage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        // Foreign Keys
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        // Business Methods
        public decimal GetTotal()
        {
            return Price * Quantity;
        }
    }
}