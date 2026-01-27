using MediatR;

namespace ShopxBase.Application.Features.Coupons.Queries.ValidateCoupon;

public class ValidateCouponQuery : IRequest<ValidateCouponResult>
{
    public string Code { get; set; }
    public decimal OrderValue { get; set; }
}

public class ValidateCouponResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
}
