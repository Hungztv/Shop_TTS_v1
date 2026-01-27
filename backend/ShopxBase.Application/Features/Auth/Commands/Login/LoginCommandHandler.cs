using MediatR;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Application.DTOs.Auth;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ISupabaseAuthService _supabaseAuthService;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        ISupabaseAuthService supabaseAuthService)
    {
        _userManager = userManager;
        _supabaseAuthService = supabaseAuthService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1) Chuẩn hóa email để đăng nhập Supabase (Supabase yêu cầu email, không phải username)
        string emailForSupabase = request.EmailOrUserName;
        if (!request.EmailOrUserName.Contains('@'))
        {
            var userByName = await _userManager.FindByNameAsync(request.EmailOrUserName);
            if (userByName == null)
                throw new UserNotFoundException("Email/Tên người dùng hoặc mật khẩu không đúng");
            emailForSupabase = userByName.Email!;
        }

        // 2) Đăng nhập qua Supabase (nguồn chính)
        var supabaseResult = await _supabaseAuthService.SignInWithPasswordAsync(emailForSupabase, request.Password);
        if (!supabaseResult.Success || supabaseResult.User == null)
        {
            var message = supabaseResult.ErrorDescription ?? supabaseResult.Error ?? "Email/Tên người dùng hoặc mật khẩu không đúng";
            throw new UnauthorizedAccessException(message);
        }

        var supabaseUser = supabaseResult.User;

        // 3) Bảo đảm user tồn tại local (AspNetUsers)
        var localUser = await _userManager.FindByIdAsync(supabaseUser.Id);
        if (localUser == null)
        {
            // Fallback: tạo user local nếu thiếu (do trigger chậm hoặc user cũ)
            localUser = new AppUser
            {
                Id = supabaseUser.Id,
                UserName = supabaseUser.Email,
                Email = supabaseUser.Email,
                FullName = supabaseUser.UserMetadata != null && supabaseUser.UserMetadata.TryGetValue("full_name", out var fullNameObj)
                    ? fullNameObj?.ToString()
                    : supabaseUser.Email,
                PhoneNumber = supabaseUser.Phone,
                Occupation = "Customer",
                Address = string.Empty,
                Avatar = string.Empty,
                token = string.Empty,
                CreatedAt = supabaseUser.CreatedAt,
                EmailConfirmed = supabaseUser.EmailConfirmedAt.HasValue
            };

            var createResult = await _userManager.CreateAsync(localUser);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Tạo user local thất bại: {errors}");
            }

            // Đảm bảo role mặc định
            await _userManager.AddToRoleAsync(localUser, "Customer");
        }

        if (localUser.IsDeleted)
            throw new UserNotFoundException("Tài khoản đã bị xóa");

        // 4) Lấy roles local để enrich response
        var roles = await _userManager.GetRolesAsync(localUser);
        if (!roles.Any())
        {
            if (!await _userManager.IsInRoleAsync(localUser, "Customer"))
            {
                await _userManager.AddToRoleAsync(localUser, "Customer");
            }
            roles = await _userManager.GetRolesAsync(localUser);
        }

        // 5) Trả về token Supabase + thông tin user local
        var expiresIn = supabaseResult.ExpiresIn ?? 3600;

        return new LoginResponseDto
        {
            AccessToken = supabaseResult.AccessToken!,
            RefreshToken = supabaseResult.RefreshToken!,
            ExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn),
            TokenType = supabaseResult.TokenType ?? "Bearer",
            User = new UserInfoDto
            {
                Id = localUser.Id,
                UserName = localUser.UserName!,
                Email = localUser.Email!,
                FullName = localUser.FullName,
                Avatar = localUser.Avatar,
                PhoneNumber = localUser.PhoneNumber,
                Roles = roles
            }
        };
    }
}
