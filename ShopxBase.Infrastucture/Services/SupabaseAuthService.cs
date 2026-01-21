using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShopxBase.Application.DTOs.Auth;
using ShopxBase.Application.Interfaces;
using ShopxBase.Application.Settings;

namespace ShopxBase.Infrastructure.Services;

/// <summary>
/// Service for interacting with Supabase Authentication API
/// </summary>
public class SupabaseAuthService : ISupabaseAuthService
{
    private readonly HttpClient _httpClient;
    private readonly SupabaseSettings _settings;
    private readonly ILogger<SupabaseAuthService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public SupabaseAuthService(
        HttpClient httpClient,
        IOptions<SupabaseSettings> settings,
        ILogger<SupabaseAuthService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true
        };

        // Configure HttpClient
        _httpClient.BaseAddress = new Uri($"{_settings.Url}/auth/v1/");
        _httpClient.DefaultRequestHeaders.Add("apikey", _settings.AnonKey);
    }

    /// <summary>
    /// Sign up a new user with email and password
    /// </summary>
    public async Task<SupabaseAuthResponse> SignUpAsync(string email, string password, Dictionary<string, object>? metadata = null)
    {
        try
        {
            var requestBody = new
            {
                email,
                password,
                data = metadata ?? new Dictionary<string, object>()
            };

            var jsonBody = JsonSerializer.Serialize(requestBody, _jsonOptions);
            _logger.LogInformation("Supabase signup request to: {Url}signup", _httpClient.BaseAddress);
            _logger.LogDebug("Supabase signup request body: {Body}", jsonBody);

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("signup", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Supabase signup response status: {StatusCode}", response.StatusCode);
            _logger.LogInformation("Supabase signup response: {Response}", responseContent);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = JsonSerializer.Deserialize<SupabaseAuthApiResponse>(responseContent, _jsonOptions);
                return new SupabaseAuthResponse
                {
                    Success = true,
                    AccessToken = apiResponse?.AccessToken,
                    RefreshToken = apiResponse?.RefreshToken,
                    ExpiresIn = apiResponse?.ExpiresIn,
                    TokenType = apiResponse?.TokenType,
                    User = apiResponse?.User,
                    Message = "Đăng ký thành công"
                };
            }
            else
            {
                var errorResponse = JsonSerializer.Deserialize<SupabaseAuthApiResponse>(responseContent, _jsonOptions);
                return new SupabaseAuthResponse
                {
                    Success = false,
                    Error = errorResponse?.Error ?? "signup_failed",
                    ErrorDescription = errorResponse?.ErrorDescription ?? errorResponse?.Message ?? responseContent
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Supabase signup");
            return new SupabaseAuthResponse
            {
                Success = false,
                Error = "exception",
                ErrorDescription = ex.Message
            };
        }
    }

    /// <summary>
    /// Sign in with email and password
    /// </summary>
    public async Task<SupabaseAuthResponse> SignInWithPasswordAsync(string email, string password)
    {
        try
        {
            var requestBody = new { email, password };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("token?grant_type=password", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("Supabase signin response: {Response}", responseContent);

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = JsonSerializer.Deserialize<SupabaseAuthApiResponse>(responseContent, _jsonOptions);
                return new SupabaseAuthResponse
                {
                    Success = true,
                    AccessToken = apiResponse?.AccessToken,
                    RefreshToken = apiResponse?.RefreshToken,
                    ExpiresIn = apiResponse?.ExpiresIn,
                    TokenType = apiResponse?.TokenType,
                    User = apiResponse?.User,
                    Message = "Đăng nhập thành công"
                };
            }
            else
            {
                var errorResponse = JsonSerializer.Deserialize<SupabaseAuthApiResponse>(responseContent, _jsonOptions);
                return new SupabaseAuthResponse
                {
                    Success = false,
                    Error = errorResponse?.Error ?? "signin_failed",
                    ErrorDescription = errorResponse?.ErrorDescription ?? errorResponse?.Message ?? "Email hoặc mật khẩu không đúng"
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Supabase signin");
            return new SupabaseAuthResponse
            {
                Success = false,
                Error = "exception",
                ErrorDescription = ex.Message
            };
        }
    }

    /// <summary>
    /// Get OAuth URL for provider
    /// </summary>
    public Task<string> GetOAuthUrlAsync(string provider, string redirectUrl)
    {
        var url = $"{_settings.Url}/auth/v1/authorize?provider={provider}&redirect_to={Uri.EscapeDataString(redirectUrl)}";
        return Task.FromResult(url);
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    public async Task<SupabaseAuthResponse> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var requestBody = new { refresh_token = refreshToken };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("token?grant_type=refresh_token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = JsonSerializer.Deserialize<SupabaseAuthApiResponse>(responseContent, _jsonOptions);
                return new SupabaseAuthResponse
                {
                    Success = true,
                    AccessToken = apiResponse?.AccessToken,
                    RefreshToken = apiResponse?.RefreshToken,
                    ExpiresIn = apiResponse?.ExpiresIn,
                    TokenType = apiResponse?.TokenType,
                    User = apiResponse?.User,
                    Message = "Token đã được làm mới"
                };
            }
            else
            {
                var errorResponse = JsonSerializer.Deserialize<SupabaseAuthApiResponse>(responseContent, _jsonOptions);
                return new SupabaseAuthResponse
                {
                    Success = false,
                    Error = errorResponse?.Error ?? "refresh_failed",
                    ErrorDescription = errorResponse?.ErrorDescription ?? "Không thể làm mới token"
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return new SupabaseAuthResponse
            {
                Success = false,
                Error = "exception",
                ErrorDescription = ex.Message
            };
        }
    }

    /// <summary>
    /// Sign out user
    /// </summary>
    public async Task<bool> SignOutAsync(string accessToken)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "logout");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Supabase signout");
            return false;
        }
    }

    /// <summary>
    /// Get user from access token
    /// </summary>
    public async Task<SupabaseUser?> GetUserAsync(string accessToken)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<SupabaseUser>(responseContent, _jsonOptions);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user from Supabase");
            return null;
        }
    }

    /// <summary>
    /// Update user metadata
    /// </summary>
    public async Task<SupabaseUser?> UpdateUserAsync(string accessToken, Dictionary<string, object> metadata)
    {
        try
        {
            var requestBody = new { data = metadata };

            var request = new HttpRequestMessage(HttpMethod.Put, "user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<SupabaseUser>(responseContent, _jsonOptions);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user in Supabase");
            return null;
        }
    }

    /// <summary>
    /// Send password reset email
    /// </summary>
    public async Task<bool> ResetPasswordForEmailAsync(string email, string redirectUrl)
    {
        try
        {
            var requestBody = new { email, redirect_to = redirectUrl };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("recover", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset email");
            return false;
        }
    }

    /// <summary>
    /// Update user password
    /// </summary>
    public async Task<bool> UpdatePasswordAsync(string accessToken, string newPassword)
    {
        try
        {
            var requestBody = new { password = newPassword };

            var request = new HttpRequestMessage(HttpMethod.Put, "user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating password");
            return false;
        }
    }
}
