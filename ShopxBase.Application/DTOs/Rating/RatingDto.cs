using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.DTOs.Rating;

/// <summary>
/// Rating response DTO
/// </summary>
public class RatingDto
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Star { get; set; }
    public string StarDisplay { get; set; } = string.Empty;
    public bool IsVerifiedPurchase { get; set; }
    public bool IsApproved { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Paginated ratings response
/// </summary>
public class RatingPagedDto
{
    public List<RatingDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}

/// <summary>
/// Rating statistics for a product
/// </summary>
public class RatingStatsDto
{
    public int ProductId { get; set; }
    public decimal AverageScore { get; set; }
    public int TotalRatings { get; set; }

    // Distribution by star
    public int FiveStarCount { get; set; }
    public int FourStarCount { get; set; }
    public int ThreeStarCount { get; set; }
    public int TwoStarCount { get; set; }
    public int OneStarCount { get; set; }

    // Percentage distribution
    public decimal FiveStarPercent => TotalRatings > 0 ? Math.Round((decimal)FiveStarCount / TotalRatings * 100, 1) : 0;
    public decimal FourStarPercent => TotalRatings > 0 ? Math.Round((decimal)FourStarCount / TotalRatings * 100, 1) : 0;
    public decimal ThreeStarPercent => TotalRatings > 0 ? Math.Round((decimal)ThreeStarCount / TotalRatings * 100, 1) : 0;
    public decimal TwoStarPercent => TotalRatings > 0 ? Math.Round((decimal)TwoStarCount / TotalRatings * 100, 1) : 0;
    public decimal OneStarPercent => TotalRatings > 0 ? Math.Round((decimal)OneStarCount / TotalRatings * 100, 1) : 0;
}


public class CreateRatingDto
{
    [Required(ErrorMessage = "Bình luận là bắt buộc")]
    [MinLength(4, ErrorMessage = "Bình luận tối thiểu 4 ký tự")]
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên là bắt buộc")]
    [MinLength(2, ErrorMessage = "Tên tối thiểu 2 ký tự")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Range(1, 5, ErrorMessage = "Đánh giá sao phải từ 1 đến 5")]
    public int Star { get; set; }

    [Required]
    public int ProductId { get; set; }
}


/// Update Rating DTO

public class UpdateRatingDto
{
    [Required(ErrorMessage = "Bình luận là bắt buộc")]
    [MinLength(4, ErrorMessage = "Bình luận tối thiểu 4 ký tự")]
    [MaxLength(1000)]
    public string Comment { get; set; } = string.Empty;

    [Required]
    [Range(1, 5, ErrorMessage = "Đánh giá sao phải từ 1 đến 5")]
    public int Star { get; set; }
}
