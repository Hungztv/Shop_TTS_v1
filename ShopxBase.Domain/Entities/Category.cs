using System.ComponentModel.DataAnnotations;

namespace Shopping.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Required, MinLength(4)]
        public string Name { get; set; }

        [Required, MinLength(4)]
        public string Description { get; set; }

        public string Slug { get; set; }

        public string Status { get; set; }

        // Navigation Properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        // Business Methods
        public int GetTotalProducts()
        {
            return Products?.Count ?? 0;
        }

        public bool IsActive()
        {
            return Status?.ToLower() == "active";
        }
    }
}