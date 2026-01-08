using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopxBase.Domain.Entities
{
    public class Rating : BaseEntity
    {
        [Required, MinLength(4)]
        public string Comment { get; set; }

        [Required, MinLength(2)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Range(1, 5)]
        public int Star { get; set; }

        public bool IsVerifiedPurchase { get; set; }
        public bool IsApproved { get; set; }

        // Foreign Keys
        public int ProductId { get; set; }
        public string UserId { get; set; }

        // Navigation Properties
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        // Business Methods
        public string GetStarDisplay()
        {
            return new string('★', Star) + new string('☆', 5 - Star);
        }
    }
}