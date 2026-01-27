namespace ShopxBase.Application.DTOs.Cart;


public class CartDto
{

    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }


    public string ProductImage { get; set; }


    public string ProductSlug { get; set; }


    public decimal Price { get; set; }

    public decimal CapitalPrice { get; set; }

    public int Quantity { get; set; }

    public int MaxQuantity { get; set; }

    public decimal Subtotal { get; set; }

    public DateTime CreatedAt { get; set; }
}
