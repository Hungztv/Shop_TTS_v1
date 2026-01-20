using MediatR;
using ShopxBase.Application.DTOs.Coupon;

namespace ShopxBase.Application.Features.Coupons.Commands.CreateCoupon;

public class CreateCouponCommand : IRequest<CouponDto>
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
    public int Status { get; set; } = 1; // Active by default
}
