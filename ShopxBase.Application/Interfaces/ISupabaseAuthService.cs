using ShopxBase.Application.DTOs.Auth;

namespace ShopxBase.Application.Interfaces;

/// <summary>
/// Interface for Supabase Authentication Service
/// </summary>
public interface ISupabaseAuthService
{
    /// <summary>
    /// Sign up a new user with email and password
    /// </summary>
    Task<SupabaseAuthResponse> SignUpAsync(string email, string password, Dictionary<string, object>? metadata = null);

    /// <summary>
    /// Sign in with email and password
    /// </summary>
    Task<SupabaseAuthResponse> SignInWithPasswordAsync(string email, string password);

    /// <summary>
    /// Sign in with OAuth provider (Google, GitHub, etc.)
    /// </summary>
    Task<string> GetOAuthUrlAsync(string provider, string redirectUrl);

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    Task<SupabaseAuthResponse> RefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Sign out user (invalidate tokens)
    /// </summary>
    Task<bool> SignOutAsync(string accessToken);

    /// <summary>
    /// Get user from access token
    /// </summary>
    Task<SupabaseUser?> GetUserAsync(string accessToken);

    /// <summary>
    /// Update user metadata
    /// </summary>
    Task<SupabaseUser?> UpdateUserAsync(string accessToken, Dictionary<string, object> metadata);

    /// <summary>
    /// Send password reset email
    /// </summary>
    Task<bool> ResetPasswordForEmailAsync(string email, string redirectUrl);

    /// <summary>
    /// Update user password
    /// </summary>
    Task<bool> UpdatePasswordAsync(string accessToken, string newPassword);
}
