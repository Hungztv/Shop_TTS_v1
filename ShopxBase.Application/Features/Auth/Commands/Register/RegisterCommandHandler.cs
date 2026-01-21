using MediatR;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Application.DTOs.Auth;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, LoginResponseDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if username exists
        var existingUserByUsername = await _userManager.FindByNameAsync(request.UserName);
        if (existingUserByUsername != null)
            throw new UserAlreadyExistsException($"Tên người dùng '{request.UserName}' đã tồn tại");

        // Check if email exists
        var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingUserByEmail != null)
            throw new UserAlreadyExistsException($"Email '{request.Email}' đã được đăng ký");

        // Create new user with all required fields to avoid NOT NULL constraint violations
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FullName = request.FullName ?? request.UserName,
            PhoneNumber = request.PhoneNumber,
            Occupation = "Customer",  // Required by database NOT NULL constraint
            Address = "",              // Required by database NOT NULL constraint
            Avatar = "",               // Required by database NOT NULL constraint
            token = "",                // Required by database NOT NULL constraint
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Đăng ký thất bại: {errors}");
        }

        // Generate tokens
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        try
        {
            // Update user with refresh token and login timestamp BEFORE role assignment
            // This avoids nested transaction issues with Supabase PgBouncer
            user.token = refreshToken;
            user.LastLoginAt = DateTime.UtcNow;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Cập nhật người dùng thất bại: {errors}");
            }

            // Assign default role AFTER update
            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Thêm vai trò thất bại: {errors}");
            }
        }
        catch (Exception ex)
        {
            // If any error occurs after user creation, delete the user to keep db clean
            await _userManager.DeleteAsync(user);
            throw new InvalidOperationException($"Lỗi trong quá trình đăng ký: {ex.Message}", ex);
        }

        // Generate access token with roles
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(user, roles);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserInfoDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                FullName = user.FullName,
                Avatar = user.Avatar,
                PhoneNumber = user.PhoneNumber,
                Roles = roles
            }
        };
    }
}
