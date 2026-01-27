using System.ComponentModel.DataAnnotations.Schema;
using ShopxBase.Domain.Exceptions;

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

        public void ValidateOrderDetail()
        {
            if (Quantity <= 0)
                throw new InvalidOrderException("Số lượng sản phẩm phải lớn hơn 0");

            if (Price < 0)
                throw new InvalidOrderException("Giá sản phẩm không được âm");

            if (string.IsNullOrWhiteSpace(ProductName))
                throw new InvalidProductException("Tên sản phẩm không được để trống");
        }
    }
}