using FluentValidation;

namespace ShopxBase.Application.Features.Shops.Commands.UpdateShop;

public class UpdateShopCommandValidator : AbstractValidator<UpdateShopCommand>
{
    public UpdateShopCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id phải lớn hơn 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên shop là bắt buộc")
            .MaximumLength(120).WithMessage("Tên shop tối đa 120 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug là bắt buộc")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug chỉ chứa chữ thường, số và dấu gạch ngang")
            .MaximumLength(120).WithMessage("Slug tối đa 120 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Mô tả tối đa 1000 ký tự");

        RuleFor(x => x.LogoUrl)
            .MaximumLength(500).WithMessage("LogoUrl tối đa 500 ký tự");

        RuleFor(x => x.CoverUrl)
            .MaximumLength(500).WithMessage("CoverUrl tối đa 500 ký tự");
    }
}
