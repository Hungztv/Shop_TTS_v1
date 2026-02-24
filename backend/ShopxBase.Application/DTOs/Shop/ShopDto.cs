namespace ShopxBase.Application.DTOs.Shop;

public class ShopDto
{
    public int Id { get; set; }
    public string OwnerUserId { get; set; }
    public int BusinessRegistrationId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
