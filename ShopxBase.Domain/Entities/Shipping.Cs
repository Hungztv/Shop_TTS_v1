using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopxBase.Domain.Entities
{
    public class Shipping : BaseEntity
    {
        [Required]
        public string Province { get; set; }

        public string District { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Status { get; set; }
    }
}