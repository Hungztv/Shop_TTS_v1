using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Application.DTOs.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên người dùng phải từ 3-50 ký tự")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6-100 ký tự")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
    [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
    public string ConfirmPassword { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? PhoneNumber { get; set; }

    public string? FullName { get; set; }
}
