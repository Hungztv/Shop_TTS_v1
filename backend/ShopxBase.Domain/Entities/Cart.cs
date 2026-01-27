using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopxBase.Domain.Entities;

public class Cart : BaseEntity
{

    [Required]
    public string UserId { get; set; }
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; }

    [ForeignKey("UserId")]
    public virtual AppUser User { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 1)
            throw new ArgumentException("Số lượng phải lớn hơn 0");

        Quantity = newQuantity;
        UpdatedAt = DateTime.UtcNow;
    }


    public decimal GetSubtotal()
    {
        return Product?.Price * Quantity ?? 0;
    }
    public bool CanUpdateQuantity(int requestedQuantity)
    {
        return Product != null && Product.Quantity >= requestedQuantity;
    }
}
