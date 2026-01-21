using MediatR;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Application.Features.Auth.DTOs;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new DomainException("Email hoặc mật khẩu không chính xác");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            throw new DomainException("Email hoặc mật khẩu không chính xác");

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // Generate token
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email!, roles);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        return new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            Roles = roles.ToList()
        };
    }
}
