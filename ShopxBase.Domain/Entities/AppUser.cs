using Microsoft.AspNetCore.Indentity;
namespace ShopxBase.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        //business properties
        public string Occupation { get; set; }
        public string? RoleId { get; set; }
        public string token { get; set; }
        //profile properties
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Avatar { get; set; }
        //Audit properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        // Navigation properties
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public virtual ICollection<CompareProduct> CompareProducts { get; set; } = new List<CompareProduct>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();


    }
}