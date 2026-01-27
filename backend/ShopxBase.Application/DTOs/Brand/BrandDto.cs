namespace ShopxBase.Application.DTOs.Brand;


public class BrandDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; }
    public string Logo { get; set; }
    public int TotalProducts { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Create Brand DTO
/// </summary>
public class CreateBrandDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; }
    public string Logo { get; set; }
}

/// <summary>
/// Update Brand DTO
/// </summary>
public class UpdateBrandDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; }
    public string Logo { get; set; }
}
