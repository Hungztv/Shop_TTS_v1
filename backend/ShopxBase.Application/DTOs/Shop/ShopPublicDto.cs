namespace ShopxBase.Application.DTOs.Shop;

public class ShopPublicDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalProducts { get; set; }
    public decimal AverageRating { get; set; }
}
