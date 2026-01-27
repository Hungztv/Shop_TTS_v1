namespace ShopxBase.Application.DTOs.Compare;

/// <summary>
/// Compare item response DTO with full product details for comparison table
/// </summary>
public class CompareItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    // Product Details
    public string ProductName { get; set; } = string.Empty;
    public string ProductSlug { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public string ProductImage { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public decimal ProductCapitalPrice { get; set; }
    public int ProductQuantity { get; set; }
    public int ProductSoldOut { get; set; }
    public bool IsInStock { get; set; }

    // Rating info
    public decimal AverageScore { get; set; }
    public int RatingCount { get; set; }

    // Brand Details
    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string BrandLogo { get; set; } = string.Empty;

    // Category Details
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Compare list summary
/// </summary>
public class CompareSummaryDto
{
    public List<CompareItemDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int MaxItems { get; set; } = 5;
    public int RemainingSlots => MaxItems - TotalItems;
}

/// <summary>
/// Request DTO for adding item to compare list
/// </summary>
public class AddToCompareDto
{
    public int ProductId { get; set; }
}
