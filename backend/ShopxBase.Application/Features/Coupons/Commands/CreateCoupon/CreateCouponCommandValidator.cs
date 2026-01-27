using FluentValidation;

namespace ShopxBase.Application.Features.Coupons.Commands.CreateCoupon;

public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên coupon là bắt buộc");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Mã coupon là bắt buộc")
            .Matches("^[A-Z0-9_-]+$").WithMessage("Mã coupon chỉ được chứa chữ in hoa, số, gạch ngang và gạch dưới");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc");

        RuleFor(x => x.DateStart)
            .NotEmpty().WithMessage("Ngày bắt đầu là bắt buộc");

        RuleFor(x => x.DateExpired)
            .NotEmpty().WithMessage("Ngày hết hạn là bắt buộc")
            .GreaterThan(x => x.DateStart).WithMessage("Ngày hết hạn phải sau ngày bắt đầu");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0).WithMessage("Giá trị giảm giá phải lớn hơn 0");

        RuleFor(x => x.DiscountValue)
            .LessThanOrEqualTo(100).WithMessage("Giảm giá theo % không được vượt quá 100%")
            .When(x => x.IsPercent);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0");

        RuleFor(x => x.MinimumOrderValue)
            .GreaterThanOrEqualTo(0).WithMessage("Giá trị đơn hàng tối thiểu không được âm");

        RuleFor(x => x.Status)
            .InclusiveBetween(0, 1).WithMessage("Status phải là 0 (Inactive) hoặc 1 (Active)");
    }
}
