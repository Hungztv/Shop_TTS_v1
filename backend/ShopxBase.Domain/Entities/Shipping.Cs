using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopxBase.Domain.Entities
{
    public class Shipping : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Ward { get; set; }

        [Required]
        [MaxLength(50)]
        public string District { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Status { get; set; }
    }
}