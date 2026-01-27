using FluentValidation;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Ratings.Commands.CreateRating;

public class CreateRatingCommandValidator : AbstractValidator<CreateRatingCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRatingCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than 0")
            .MustAsync(ProductExists)
            .WithMessage("Product does not exist or is deleted");

        RuleFor(x => x.Star)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5 stars");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Comment is required")
            .MinimumLength(4)
            .WithMessage("Comment must be at least 4 characters")
            .MaximumLength(1000)
            .WithMessage("Comment must not exceed 1000 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters");
    }

    private async Task<bool> ProductExists(int productId, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        return product != null && !product.IsDeleted;
    }
}
