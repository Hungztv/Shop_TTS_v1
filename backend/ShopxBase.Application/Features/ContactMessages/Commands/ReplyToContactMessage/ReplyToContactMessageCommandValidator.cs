using FluentValidation;

namespace ShopxBase.Application.Features.ContactMessages.Commands.ReplyToContactMessage;

public class ReplyToContactMessageCommandValidator : AbstractValidator<ReplyToContactMessageCommand>
{
    public ReplyToContactMessageCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID không hợp lệ");

        RuleFor(x => x.ReplyMessage)
            .NotEmpty().WithMessage("Nội dung phản hồi không được để trống")
            .MaximumLength(2000).WithMessage("Nội dung phản hồi không được vượt quá 2000 ký tự");
    }
}
