namespace ShopxBase.Application.DTOs.Coupon;

/// <summary>
/// Coupon DTO for read operations
/// </summary>
public class CouponDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateExpired { get; set; }
    public decimal DiscountValue { get; set; }
    public bool IsPercent { get; set; }
    public int Quantity { get; set; }
    public int UsedCount { get; set; }
    public int AvailableCount { get; set; }
    public decimal MinimumOrderValue { get; set; }
    public int Status { get; set; }
    public bool IsValid { get; set; }
    public bool IsExpired { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Create Coupon DTO
/// </summary>
public class CreateCouponDto
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateExpired { get; set; }
    public decimal DiscountValue { get; set; }
    public bool IsPercent { get; set; }
    public int Quantity { get; set; }
    public decimal MinimumOrderValue { get; set; }
    public int Status { get; set; }
}

/// <summary>
/// Update Coupon DTO
/// </summary>
public class UpdateCouponDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateExpired { get; set; }
    public decimal DiscountValue { get; set; }
    public bool IsPercent { get; set; }
    public int Quantity { get; set; }
    public decimal MinimumOrderValue { get; set; }
    public int Status { get; set; }
}

/// <summary>
/// Validate Coupon DTO
/// </summary>
public class ValidateCouponDto
{
    public string Code { get; set; }
    public decimal OrderValue { get; set; }
}
