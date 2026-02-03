using FluentValidation;

namespace ShopxBase.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        // Id is set automatically in controller, validation removed

        When(x => !string.IsNullOrEmpty(x.FullName), () =>
        {
            RuleFor(x => x.FullName)
                .MaximumLength(200).WithMessage("Họ tên không được quá 200 ký tự");
        });

        When(x => !string.IsNullOrEmpty(x.Occupation), () =>
        {
            RuleFor(x => x.Occupation)
                .MaximumLength(100).WithMessage("Nghề nghiệp không được quá 100 ký tự");
        });

        When(x => !string.IsNullOrEmpty(x.Address), () =>
        {
            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Địa chỉ không được quá 500 ký tự");
        });

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Ngày sinh phải trong quá khứ")
            .When(x => x.DateOfBirth.HasValue);
    }
}
