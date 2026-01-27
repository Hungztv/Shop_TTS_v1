using System.ComponentModel.DataAnnotations;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Domain.Entities
{
    public class Brand : BaseEntity
    {
        [Required, MinLength(4)]
        public string Name { get; set; }

        [Required, MinLength(4)]
        public string Description { get; set; }

        public string Slug { get; set; }

        public string Status { get; set; }

        public string Logo { get; set; }

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

        public void ValidateBrand()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length < 4)
                throw new BrandNotFoundException("Tên thương hiệu phải có ít nhất 4 ký tự");

            if (string.IsNullOrWhiteSpace(Description) || Description.Length < 4)
                throw new BrandNotFoundException("Mô tả thương hiệu phải có ít nhất 4 ký tự");
        }
    }
}