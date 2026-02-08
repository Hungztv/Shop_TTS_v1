using MediatR;
using ShopxBase.Application.DTOs.Coupon;

namespace ShopxBase.Application.Features.Coupons.Queries.GetCouponById;

public class GetCouponByIdQuery : IRequest<CouponDto?>
{
    public int Id { get; set; }
}
