using MediatR;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Application.DTOs.Auth;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Validate refresh token
        var userId = await _jwtTokenService.ValidateRefreshTokenAsync(request.RefreshToken);

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Refresh token không hợp lệ hoặc đã hết hạn");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.IsDeleted)
            throw new UnauthorizedAccessException("Người dùng không tồn tại");

        // Generate new tokens
        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = await _jwtTokenService.GenerateAccessTokenAsync(user, roles);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        // Update refresh token
        user.token = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
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
