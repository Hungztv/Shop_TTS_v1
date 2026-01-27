using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Domain.Entities;

public class Product : BaseEntity
{
    //Basic info
    [Required, MinLength(4)]
    public string Name { get; set; }
    public string Slug { get; set; }
    [Required, MinLength(4)]
    public string Description { get; set; }
    //Pricing
    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal CapitalPrice { get; set; }
    public int Quantity { get; set; }
    public int SoldOut { get; set; }
    // Media
    public string Image { get; set; }

    // Rating Statistics (denormalized for performance)
    [Column(TypeName = "decimal(3,2)")]
    public decimal AverageScore { get; set; } = 0;
    public int RatingCount { get; set; } = 0;

    // Foreign Keys
    public int BrandId { get; set; }
    public int CategoryId { get; set; }

    // Navigation Properties
    public virtual Brand Brand { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    public virtual ICollection<CompareProduct> CompareProducts { get; set; } = new List<CompareProduct>();
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    //Business Methods
    public decimal GetaverageRating()
    {
        if (Ratings == null || Ratings.Any())
            return 0;
        return (decimal)Ratings.Average(r => r.Star);
    }
    public int GetTotalReviews()
    {
        return Ratings?.Count ?? 0;
    }

    public bool IsInStock()
    {
        return Quantity > 0;
    }

    public bool CanPurchase(int requestedQuantity)
    {
        return Quantity >= requestedQuantity;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity > Quantity)
            throw InsufficientStockException.For(Name, quantity, Quantity);

        Quantity -= quantity;
        SoldOut += quantity;
    }

    public void RestoreStock(int quantity)
    {
        Quantity += quantity;
        SoldOut -= quantity;
    }
}
