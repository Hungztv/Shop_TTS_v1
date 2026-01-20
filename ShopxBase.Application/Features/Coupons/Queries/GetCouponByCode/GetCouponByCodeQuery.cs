using MediatR;
using ShopxBase.Application.DTOs.Coupon;

namespace ShopxBase.Application.Features.Coupons.Queries.GetCouponByCode;

public class GetCouponByCodeQuery : IRequest<CouponDto?>
{
    public string Code { get; set; }
}
