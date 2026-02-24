using FluentValidation;

namespace ShopxBase.Application.Features.BusinessRegistrations.Commands.RejectBusinessRegistration;

public class RejectBusinessRegistrationCommandValidator : AbstractValidator<RejectBusinessRegistrationCommand>
{
    public RejectBusinessRegistrationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id phải lớn hơn 0");

        RuleFor(x => x.RejectReason)
            .NotEmpty().WithMessage("RejectReason là bắt buộc")
            .MaximumLength(500).WithMessage("RejectReason tối đa 500 ký tự");
    }
}
