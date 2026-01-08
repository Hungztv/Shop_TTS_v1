namespace ShopxBase.Application.DTOs;

/// <summary>
/// Product Data Transfer Object
/// </summary>
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }
    public string Category { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Create Product Request DTO
/// </summary>
public class CreateProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }
    public string Category { get; set; }
}

/// <summary>
/// Update Product Request DTO
/// </summary>
public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }
}
