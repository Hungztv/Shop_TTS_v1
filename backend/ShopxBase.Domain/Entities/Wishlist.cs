using System.ComponentModel.DataAnnotations.Schema;

namespace ShopxBase.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        // Foreign Keys
        public int ProductId { get; set; }
        public string UserId { get; set; }

        // Navigation Properties
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
    }
}