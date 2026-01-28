namespace ShopxBase.Application.DTOs.Coupon;


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

public class ValidateCouponDto
{
    public string Code { get; set; }
    public decimal OrderValue { get; set; }
}
