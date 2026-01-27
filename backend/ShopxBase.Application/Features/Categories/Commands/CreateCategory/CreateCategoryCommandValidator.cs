using FluentValidation;

namespace ShopxBase.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên danh mục là bắt buộc")
            .MinimumLength(4).WithMessage("Tên danh mục tối thiểu 4 ký tự")
            .MaximumLength(200).WithMessage("Tên danh mục tối đa 200 ký tự");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả là bắt buộc")
            .MinimumLength(4).WithMessage("Mô tả tối thiểu 4 ký tự")
            .MaximumLength(500).WithMessage("Mô tả tối đa 500 ký tự");

        RuleFor(x => x.Slug)
            .MaximumLength(250).WithMessage("Slug tối đa 250 ký tự");

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status tối đa 50 ký tự");
    }
}
