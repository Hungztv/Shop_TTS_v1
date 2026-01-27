using FluentValidation;

namespace ShopxBase.Application.Features.Cart.Commands.UpdateCartQuantity;


public class UpdateCartQuantityCommandValidator : AbstractValidator<UpdateCartQuantityCommand>
{
    public UpdateCartQuantityCommandValidator()
    {
        RuleFor(x => x.CartId)
            .GreaterThan(0).WithMessage("CartId phải lớn hơn 0");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId không được để trống");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("Số lượng phải lớn hơn hoặc bằng 1")
            .LessThanOrEqualTo(999).WithMessage("Số lượng không được vượt quá 999");
    }
}
