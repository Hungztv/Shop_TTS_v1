namespace ShopxBase.Application.DTOs.Cart;


public class CartSummaryDto
{

    public List<CartDto> Items { get; set; } = new();

    public int TotalItems { get; set; }

    public decimal TotalPrice { get; set; }

    public int UniqueProducts { get; set; }
}
