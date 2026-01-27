using FluentValidation;

namespace ShopxBase.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Tên người dùng là bắt buộc")
            .MinimumLength(3).WithMessage("Tên người dùng phải có ít nhất 3 ký tự")
            .MaximumLength(50).WithMessage("Tên người dùng không được vượt quá 50 ký tự");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là bắt buộc")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .Must(BeAValidEmailFormat).WithMessage("Email chứa ký tự không được hỗ trợ");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu là bắt buộc")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
            .MaximumLength(100).WithMessage("Mật khẩu không được vượt quá 100 ký tự");
    }

    private bool BeAValidEmailFormat(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Supabase có thể không chấp nhận email domain có dấu gạch ngang hoặc ký tự đặc biệt
        // Validation này có thể bỏ qua nếu Supabase settings đã allow
        var parts = email.Split('@');
        if (parts.Length != 2)
            return false;

        var domain = parts[1];

        // Check domain có ký tự hợp lệ (cho phép dấu gạch ngang, chấm, chữ số)
        // Nếu Supabase vẫn reject, có thể điều chỉnh regex này
        return System.Text.RegularExpressions.Regex.IsMatch(
            domain,
            @"^[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?(\.[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)*$"
        );
    }
}
