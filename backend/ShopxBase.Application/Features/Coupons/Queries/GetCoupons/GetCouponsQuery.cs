using MediatR;
using ShopxBase.Application.DTOs.Coupon;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Coupons.Queries.GetCoupons;

public class GetCouponsQuery : IRequest<PaginatedResult<CouponDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public int? Status { get; set; }
    public bool? IsValid { get; set; }
    public bool? IsExpired { get; set; }
}
