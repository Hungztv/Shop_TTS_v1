using FluentValidation;

namespace ShopxBase.Application.Features.Sliders.Commands.CreateSlider;

public class CreateSliderCommandValidator : AbstractValidator<CreateSliderCommand>
{
    public CreateSliderCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên slider không được để trống")
            .MaximumLength(100).WithMessage("Tên slider không được vượt quá 100 ký tự");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề không được để trống")
            .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự");

        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Hình ảnh không được để trống");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");
    }
}
