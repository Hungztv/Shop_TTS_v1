using FluentValidation;

namespace ShopxBase.Application.Features.Sliders.Commands.UpdateSlider;

public class UpdateSliderCommandValidator : AbstractValidator<UpdateSliderCommand>
{
    public UpdateSliderCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID slider không hợp lệ");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Tên slider không được vượt quá 100 ký tự")
            .When(x => x.Name != null);

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự")
            .When(x => x.Title != null);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự")
            .When(x => x.Description != null);
    }
}
