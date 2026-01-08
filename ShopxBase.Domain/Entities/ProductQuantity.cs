using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Domain.Entities
{
    public class ProductQuantity : BaseEntity
    {
        public int ProductId { get; set; }

        public int QuantityChange { get; set; }

        public int QuantityBefore { get; set; }

        public int QuantityAfter { get; set; }

        public string Reason { get; set; }

        public string Note { get; set; }

        public string CreatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}