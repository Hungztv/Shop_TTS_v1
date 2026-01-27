namespace ShopxBase.Application.DTOs.Wishlist;

/// <summary>
/// Wishlist item response DTO
/// </summary>
public class WishlistDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSlug { get; set; } = string.Empty;
    public string ProductImage { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public decimal ProductCapitalPrice { get; set; }
    public int ProductQuantity { get; set; }
    public bool IsInStock { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Wishlist summary with all items
/// </summary>
public class WishlistSummaryDto
{
    public List<WishlistDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
}

/// <summary>
/// Request DTO for adding item to wishlist
/// </summary>
public class AddToWishlistDto
{
    public int ProductId { get; set; }
}
