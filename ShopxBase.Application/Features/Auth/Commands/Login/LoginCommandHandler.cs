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
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Find user by email or username
        var user = await _userManager.FindByEmailAsync(request.EmailOrUserName)
                   ?? await _userManager.FindByNameAsync(request.EmailOrUserName);

        if (user == null)
            throw new UserNotFoundException("Email/Tên người dùng hoặc mật khẩu không đúng");

        // Check if account is deleted
        if (user.IsDeleted)
            throw new UserNotFoundException("Tài khoản đã bị xóa");

        // Check password using UserManager
        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            throw new UnauthorizedAccessException("Email/Tên người dùng hoặc mật khẩu không đúng");

        // Generate tokens
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(user, roles);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // Update user with refresh token and last login
        user.token = refreshToken;
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

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
