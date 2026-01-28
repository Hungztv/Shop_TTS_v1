using ShopxBase.Application.DTOs.Auth;

namespace ShopxBase.Application.Interfaces;


public interface ISupabaseAuthService
{
 
    Task<SupabaseAuthResponse> SignUpAsync(string email, string password, Dictionary<string, object>? metadata = null);

    Task<SupabaseAuthResponse> SignInWithPasswordAsync(string email, string password);

    Task<string> GetOAuthUrlAsync(string provider, string redirectUrl);

    Task<SupabaseAuthResponse> RefreshTokenAsync(string refreshToken);

    Task<bool> SignOutAsync(string accessToken);

    Task<SupabaseUser?> GetUserAsync(string accessToken);

    Task<SupabaseUser?> UpdateUserAsync(string accessToken, Dictionary<string, object> metadata);

    Task<bool> ResetPasswordForEmailAsync(string email, string redirectUrl);

    Task<bool> UpdatePasswordAsync(string accessToken, string newPassword);
}
