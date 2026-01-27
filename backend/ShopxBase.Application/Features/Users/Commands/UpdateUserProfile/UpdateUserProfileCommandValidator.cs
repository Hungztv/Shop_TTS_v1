using FluentValidation;

namespace ShopxBase.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User Id là bắt buộc");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên là bắt buộc");

        RuleFor(x => x.Occupation)
            .NotEmpty().WithMessage("Nghề nghiệp là bắt buộc");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Địa chỉ là bắt buộc");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Ngày sinh phải trong quá khứ")
            .When(x => x.DateOfBirth.HasValue);
    }
}
