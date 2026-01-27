using FluentValidation;

namespace ShopxBase.Application.Features.Ratings.Commands.UpdateRating;

public class UpdateRatingCommandValidator : AbstractValidator<UpdateRatingCommand>
{
    public UpdateRatingCommandValidator()
    {
        RuleFor(x => x.RatingId)
            .GreaterThan(0)
            .WithMessage("Rating ID must be greater than 0");

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
    }
}
