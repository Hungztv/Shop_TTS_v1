namespace ShopxBase.Domain.Entities;

/// <summary>
/// Product entity
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }
    public string Category { get; set; }
    public bool IsAvailable { get; set; }

    public Product()
    {
        IsAvailable = true;
    }

    public Product(string name, string description, decimal price, int quantity, string sku, string category)
        : this()
    {
        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
        Sku = sku;
        Category = category;
    }
}
