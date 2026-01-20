using FluentValidation;

namespace ShopxBase.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommandValidator : AbstractValidator<DeleteCouponCommand>
{
    public DeleteCouponCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Coupon Id phải lớn hơn 0");
    }
}
