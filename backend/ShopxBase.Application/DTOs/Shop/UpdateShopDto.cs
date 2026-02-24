namespace ShopxBase.Application.DTOs.Shop;

public class UpdateShopDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
}
