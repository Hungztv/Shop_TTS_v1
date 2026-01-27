using FluentValidation;

namespace ShopxBase.Application.Features.Cart.Commands.AddToCart;

public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId không được để trống");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId phải lớn hơn 0");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng phải lớn hơn hoặc bằng 1")
            .LessThanOrEqualTo(999).WithMessage("Số lượng không được vượt quá 999");
    }
}
