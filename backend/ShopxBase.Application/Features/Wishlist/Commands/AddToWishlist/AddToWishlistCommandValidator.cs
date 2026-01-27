using FluentValidation;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Wishlist.Commands.AddToWishlist;

public class AddToWishlistCommandValidator : AbstractValidator<AddToWishlistCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddToWishlistCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0")
            .MustAsync(ProductExists)
            .WithMessage("Product does not exist or is deleted");
    }

    private async Task<bool> ProductExists(int productId, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        return product != null && !product.IsDeleted;
    }
}
