namespace ShopxBase.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(string userId, string email, IList<string> roles);
    string GenerateRefreshToken();
}