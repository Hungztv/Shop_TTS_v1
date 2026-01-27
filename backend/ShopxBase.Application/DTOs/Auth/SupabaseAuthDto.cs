using System.Text.Json.Serialization;

namespace ShopxBase.Application.DTOs.Auth;

/// <summary>
/// Supabase Auth response DTO
/// </summary>
public class SupabaseAuthResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public int? ExpiresIn { get; set; }
    public string? TokenType { get; set; }
    public SupabaseUser? User { get; set; }
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
}

/// <summary>
/// Supabase User DTO
/// </summary>
public class SupabaseUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("aud")]
    public string Aud { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("email_confirmed_at")]
    public DateTime? EmailConfirmedAt { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("phone_confirmed_at")]
    public DateTime? PhoneConfirmedAt { get; set; }

    [JsonPropertyName("confirmed_at")]
    public DateTime? ConfirmedAt { get; set; }

    [JsonPropertyName("last_sign_in_at")]
    public DateTime? LastSignInAt { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("user_metadata")]
    public Dictionary<string, object>? UserMetadata { get; set; }

    [JsonPropertyName("app_metadata")]
    public Dictionary<string, object>? AppMetadata { get; set; }
}

/// <summary>
/// Supabase raw API response for auth endpoints
/// </summary>
public class SupabaseAuthApiResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }

    [JsonPropertyName("expires_at")]
    public long? ExpiresAt { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("user")]
    public SupabaseUser? User { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }

    [JsonPropertyName("msg")]
    public string? Message { get; set; }
}

/// <summary>
/// DTO for Supabase Sign Up request
/// </summary>
public class SupabaseSignUpRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Phone { get; set; }
}

/// <summary>
/// DTO for Supabase Sign In request
/// </summary>
public class SupabaseSignInRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// DTO for Supabase Refresh Token request
/// </summary>
public class SupabaseRefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// DTO for Supabase Password Reset request
/// </summary>
public class SupabasePasswordResetRequest
{
    public string Email { get; set; } = string.Empty;
    public string? RedirectUrl { get; set; }
}

/// <summary>
/// DTO for Supabase Update Password request
/// </summary>
public class SupabaseUpdatePasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}
