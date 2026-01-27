using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.DTOs.Cart;

public class AddToCartDto
{
    [Required(ErrorMessage = "ProductId là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductId phải lớn hơn 0")]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(1, 999, ErrorMessage = "Số lượng phải từ 1 đến 999")]
    public int Quantity { get; set; }
}
