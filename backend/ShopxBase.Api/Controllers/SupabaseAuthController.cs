using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs.Auth;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ShopxBase.Api.Controllers;

/// <summary>
/// Controller for Supabase Authentication
/// Proxies authentication requests to Supabase Auth API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SupabaseAuthController : ControllerBase
{
    private readonly ISupabaseAuthService _authService;
    private readonly ILogger<SupabaseAuthController> _logger;
    private readonly UserManager<AppUser> _userManager;

    public SupabaseAuthController(
        ISupabaseAuthService authService,
        ILogger<SupabaseAuthController> logger,
        UserManager<AppUser> userManager)
    {
        _authService = authService;
        _logger = logger;
        _userManager = userManager;
    }

    /// <summary>
    /// Sign up with email and password
    /// </summary>
    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SupabaseSignUpRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { success = false, message = "Email và mật khẩu là bắt buộc" });
        }

        // Chặn trùng email/username ở local trước khi gọi Supabase để tránh lỗi 23505 từ trigger
        var existingByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingByEmail != null)
        {
            return Conflict(new { success = false, error = "email_exists", message = "Email đã được sử dụng" });
        }

        var userNameToCheck = string.IsNullOrWhiteSpace(request.FullName) ? request.Email : request.FullName;
        if (!string.IsNullOrWhiteSpace(userNameToCheck))
        {
            var existingByUserName = await _userManager.FindByNameAsync(userNameToCheck);
            if (existingByUserName != null)
            {
                return Conflict(new { success = false, error = "username_exists", message = "Tên người dùng đã tồn tại, vui lòng chọn tên khác" });
            }
        }

        var metadata = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(request.FullName))
            metadata["full_name"] = request.FullName;
        if (!string.IsNullOrEmpty(request.Phone))
            metadata["phone"] = request.Phone;

        var result = await _authService.SignUpAsync(request.Email, request.Password, metadata);

        if (result.Success)
        {
            return Ok(new
            {
                success = true,
                message = result.Message,
                accessToken = result.AccessToken,
                refreshToken = result.RefreshToken,
                expiresIn = result.ExpiresIn,
                user = result.User
            });
        }

        // Xử lý lỗi domain email không được phép
        var errorMsg = result.ErrorDescription ?? result.Error ?? "Đăng ký thất bại";
        if (errorMsg.Contains("invalid", StringComparison.OrdinalIgnoreCase) &&
            errorMsg.Contains("email", StringComparison.OrdinalIgnoreCase))
        {
            var domain = request.Email.Split('@').LastOrDefault();
            return BadRequest(new
            {
                success = false,
                error = "email_domain_not_allowed",
                message = $"Email với domain '{domain}' chưa được hỗ trợ. Vui lòng sử dụng email từ các nhà cung cấp phổ biến (Gmail, Outlook, v.v.) hoặc liên hệ quản trị viên."
            });
        }

        return BadRequest(new
        {
            success = false,
            error = result.Error,
            message = errorMsg
        });
    }

    /// <summary>
    /// Sign in with email and password
    /// </summary>
    [HttpPost("signin")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SupabaseSignInRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { success = false, message = "Email và mật khẩu là bắt buộc" });
        }

        var result = await _authService.SignInWithPasswordAsync(request.Email, request.Password);

        if (result.Success)
        {
            return Ok(new
            {
                success = true,
                message = result.Message,
                accessToken = result.AccessToken,
                refreshToken = result.RefreshToken,
                expiresIn = result.ExpiresIn,
                tokenType = result.TokenType,
                user = result.User
            });
        }

        return Unauthorized(new
        {
            success = false,
            error = result.Error,
            message = result.ErrorDescription
        });
    }

    /// <summary>
    /// Refresh access token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] SupabaseRefreshTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return BadRequest(new { success = false, message = "Refresh token là bắt buộc" });
        }

        var result = await _authService.RefreshTokenAsync(request.RefreshToken);

        if (result.Success)
        {
            return Ok(new
            {
                success = true,
                message = result.Message,
                accessToken = result.AccessToken,
                refreshToken = result.RefreshToken,
                expiresIn = result.ExpiresIn,
                user = result.User
            });
        }

        return Unauthorized(new
        {
            success = false,
            error = result.Error,
            message = result.ErrorDescription
        });
    }

    /// <summary>
    /// Sign out user
    /// </summary>
    [HttpPost("signout")]
    [Authorize]
    public async Task<IActionResult> SignOut()
    {
        var accessToken = GetAccessToken();
        if (string.IsNullOrEmpty(accessToken))
        {
            return BadRequest(new { success = false, message = "Access token không hợp lệ" });
        }

        var result = await _authService.SignOutAsync(accessToken);

        if (result)
        {
            return Ok(new { success = true, message = "Đăng xuất thành công" });
        }

        return BadRequest(new { success = false, message = "Đăng xuất thất bại" });
    }

    /// <summary>
    /// Get current user info
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var accessToken = GetAccessToken();
        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized(new { success = false, message = "Access token không hợp lệ" });
        }

        var user = await _authService.GetUserAsync(accessToken);

        if (user != null)
        {
            return Ok(new
            {
                success = true,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    phone = user.Phone,
                    emailConfirmed = user.EmailConfirmedAt.HasValue,
                    phoneConfirmed = user.PhoneConfirmedAt.HasValue,
                    lastSignInAt = user.LastSignInAt,
                    createdAt = user.CreatedAt,
                    metadata = user.UserMetadata
                }
            });
        }

        return Unauthorized(new { success = false, message = "Không thể lấy thông tin người dùng" });
    }

    /// <summary>
    /// Get current user info with roles from ASP.NET Core Identity
    /// Use this endpoint to get roles for authorization
    /// </summary>
    [HttpGet("me/with-roles")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserWithRoles()
    {
        var accessToken = GetAccessToken();
        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized(new { success = false, message = "Access token không hợp lệ" });
        }

        var supabaseUser = await _authService.GetUserAsync(accessToken);
        if (supabaseUser == null)
        {
            return Unauthorized(new { success = false, message = "Không thể lấy thông tin người dùng từ Supabase" });
        }

        // Fetch user from AppUser to get roles
        var appUser = await _userManager.FindByEmailAsync(supabaseUser.Email);
        if (appUser == null)
        {
            return NotFound(new { success = false, message = "Người dùng không tồn tại trong hệ thống" });
        }

        // Get roles from AspNetUserRoles
        var roles = await _userManager.GetRolesAsync(appUser);

        return Ok(new
        {
            success = true,
            user = new
            {
                id = supabaseUser.Id,
                email = supabaseUser.Email,
                phone = supabaseUser.Phone,
                emailConfirmed = supabaseUser.EmailConfirmedAt.HasValue,
                phoneConfirmed = supabaseUser.PhoneConfirmedAt.HasValue,
                lastSignInAt = supabaseUser.LastSignInAt,
                createdAt = supabaseUser.CreatedAt,
                roles = roles.ToList(),
                metadata = supabaseUser.UserMetadata
            }
        });
    }

    /// <summary>
    /// Update user metadata
    /// </summary>
    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] Dictionary<string, object> metadata)
    {
        var accessToken = GetAccessToken();
        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized(new { success = false, message = "Access token không hợp lệ" });
        }

        var user = await _authService.UpdateUserAsync(accessToken, metadata);

        if (user != null)
        {
            return Ok(new
            {
                success = true,
                message = "Cập nhật thành công",
                user
            });
        }

        return BadRequest(new { success = false, message = "Cập nhật thất bại" });
    }

    /// <summary>
    /// Send password reset email
    /// </summary>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] SupabasePasswordResetRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new { success = false, message = "Email là bắt buộc" });
        }

        var redirectUrl = request.RedirectUrl ?? $"{Request.Scheme}://{Request.Host}/reset-password";
        var result = await _authService.ResetPasswordForEmailAsync(request.Email, redirectUrl);

        // Always return success to prevent email enumeration
        return Ok(new
        {
            success = true,
            message = "Nếu email tồn tại, chúng tôi đã gửi link đặt lại mật khẩu"
        });
    }

    /// <summary>
    /// Update password (for authenticated users)
    /// </summary>
    [HttpPost("update-password")]
    [Authorize]
    public async Task<IActionResult> UpdatePassword([FromBody] SupabaseUpdatePasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.NewPassword))
        {
            return BadRequest(new { success = false, message = "Mật khẩu mới là bắt buộc" });
        }

        if (request.NewPassword.Length < 6)
        {
            return BadRequest(new { success = false, message = "Mật khẩu phải có ít nhất 6 ký tự" });
        }

        var accessToken = GetAccessToken();
        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized(new { success = false, message = "Access token không hợp lệ" });
        }

        var result = await _authService.UpdatePasswordAsync(accessToken, request.NewPassword);

        if (result)
        {
            return Ok(new { success = true, message = "Đổi mật khẩu thành công" });
        }

        return BadRequest(new { success = false, message = "Đổi mật khẩu thất bại" });
    }

    /// <summary>
    /// Get OAuth URL for provider
    /// </summary>
    [HttpGet("oauth/{provider}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOAuthUrl(string provider, [FromQuery] string? redirectUrl)
    {
        var validProviders = new[] { "google", "github", "facebook", "twitter", "apple", "discord" };
        if (!validProviders.Contains(provider.ToLower()))
        {
            return BadRequest(new { success = false, message = $"Provider không hỗ trợ: {provider}" });
        }

        var redirect = redirectUrl ?? $"{Request.Scheme}://{Request.Host}/auth/callback";
        var url = await _authService.GetOAuthUrlAsync(provider.ToLower(), redirect);

        return Ok(new
        {
            success = true,
            provider,
            url
        });
    }

    /// <summary>
    /// Helper to extract access token from Authorization header
    /// </summary>
    private string? GetAccessToken()
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return null;
        }
        return authHeader.Substring(7);
    }
}
