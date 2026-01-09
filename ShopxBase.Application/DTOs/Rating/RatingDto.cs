using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.DTOs.Rating;

/// <summary>
/// Rating DTO for read operations
/// </summary>
public class RatingDto
{
    public int Id { get; set; }
    public string Comment { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Star { get; set; }
    public string StarDisplay { get; set; }
    public bool IsVerifiedPurchase { get; set; }
    public bool IsApproved { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Create Rating DTO
/// </summary>
public class CreateRatingDto
{
    [Required(ErrorMessage = "Bình luận là bắt buộc")]
    [MinLength(4, ErrorMessage = "Bình luận tối thiểu 4 ký tự")]
    [MaxLength(1000)]
    public string Comment { get; set; }

    [Required(ErrorMessage = "Tên là bắt buộc")]
    [MinLength(2, ErrorMessage = "Tên tối thiểu 2 ký tự")]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Đánh giá sao phải từ 1 đến 5")]
    public int Star { get; set; }

    [Required]
    public int ProductId { get; set; }

    public string UserId { get; set; }
}

/// <summary>
/// Update Rating DTO
/// </summary>
public class UpdateRatingDto
{
    public int Id { get; set; }
    public string Comment { get; set; }
    public int Star { get; set; }
}
