using MediatR;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Application.Features.Auth.DTOs;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(
        UserManager<AppUser> userManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check email exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new DomainException("Email đã được sử dụng");

        // Create user
        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new DomainException($"Đăng ký thất bại: {errors}");
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, "Customer");

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
