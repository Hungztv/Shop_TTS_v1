namespace ShopxBase.Application.Settings;

/// <summary>
/// Configuration settings for Supabase integration
/// </summary>
public class SupabaseSettings
{
    /// <summary>
    /// Supabase project URL (e.g., https://xxxxx.supabase.co)
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Supabase anonymous/public key for client-side operations
    /// </summary>
    public string AnonKey { get; set; } = string.Empty;

    /// <summary>
    /// Supabase JWT secret for token verification
    /// Found in: Supabase Dashboard > Settings > API > JWT Secret
    /// </summary>
    public string JwtSecret { get; set; } = string.Empty;
}
