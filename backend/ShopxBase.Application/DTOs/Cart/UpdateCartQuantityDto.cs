using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.DTOs.Cart;

public class UpdateCartQuantityDto
{
    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(1, 999, ErrorMessage = "Số lượng phải từ 1 đến 999")]
    public int Quantity { get; set; }
}
