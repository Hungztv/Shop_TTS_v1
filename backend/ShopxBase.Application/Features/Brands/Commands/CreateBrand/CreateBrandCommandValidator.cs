using FluentValidation;

namespace ShopxBase.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên thương hiệu là bắt buộc")
            .MinimumLength(4).WithMessage("Tên thương hiệu tối thiểu 4 ký tự")
            .MaximumLength(200).WithMessage("Tên thương hiệu tối đa 200 ký tự");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc")
            .MinimumLength(4).WithMessage("Mô tả tối thiểu 4 ký tự")
            .MaximumLength(500).WithMessage("Mô tả tối đa 500 ký tự");

        RuleFor(x => x.Slug)
            .MaximumLength(250).WithMessage("Slug tối đa 250 ký tự");

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status tối đa 50 ký tự");

        RuleFor(x => x.Logo)
            .MaximumLength(500).WithMessage("Logo URL tối đa 500 ký tự");
    }
}
