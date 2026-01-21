using ShopxBase.Domain.Entities;

namespace ShopxBase.Application.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateAccessTokenAsync(AppUser user, IList<string> roles);
    string GenerateRefreshToken();
    Task<string?> ValidateRefreshTokenAsync(string refreshToken);
}
