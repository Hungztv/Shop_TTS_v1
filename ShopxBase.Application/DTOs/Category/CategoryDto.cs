using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.DTOs.Category;

/// <summary>
/// Category DTO for read operations
/// </summary>
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; }
    public int TotalProducts { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Create Category DTO
/// </summary>
public class CreateCategoryDto
{
    [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
    [MinLength(4, ErrorMessage = "Tên danh mục tối thiểu 4 ký tự")]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    [MinLength(4, ErrorMessage = "Mô tả tối thiểu 4 ký tự")]
    [MaxLength(500)]
    public string Description { get; set; }

    [MaxLength(250)]
    public string Slug { get; set; }

    [MaxLength(50)]
    public string Status { get; set; }
}

/// <summary>
/// Update Category DTO
/// </summary>
public class UpdateCategoryDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
    [MinLength(4, ErrorMessage = "Tên danh mục tối thiểu 4 ký tự")]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    [MinLength(4, ErrorMessage = "Mô tả tối thiểu 4 ký tự")]
    [MaxLength(500)]
    public string Description { get; set; }

    [MaxLength(250)]
    public string Slug { get; set; }

    [MaxLength(50)]
    public string Status { get; set; }
}
