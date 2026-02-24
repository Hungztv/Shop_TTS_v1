using FluentValidation;

namespace ShopxBase.Application.Features.BusinessRegistrations.Commands.CreateBusinessRegistration;

public class CreateBusinessRegistrationCommandValidator : AbstractValidator<CreateBusinessRegistrationCommand>
{
    public CreateBusinessRegistrationCommandValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("CompanyName là bắt buộc")
            .MaximumLength(200).WithMessage("CompanyName tối đa 200 ký tự");

        RuleFor(x => x.TaxCode)
            .NotEmpty().WithMessage("TaxCode là bắt buộc")
            .MaximumLength(50).WithMessage("TaxCode tối đa 50 ký tự");

        RuleFor(x => x.OwnerName)
            .NotEmpty().WithMessage("OwnerName là bắt buộc")
            .MaximumLength(150).WithMessage("OwnerName tối đa 150 ký tự");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là bắt buộc")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .MaximumLength(150).WithMessage("Email tối đa 150 ký tự");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone là bắt buộc")
            .MaximumLength(30).WithMessage("Phone tối đa 30 ký tự");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address là bắt buộc")
            .MaximumLength(300).WithMessage("Address tối đa 300 ký tự");
    }
}
