using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShopxBase.Application.Interfaces;

namespace ShopxBase.Infrastructure.Services;

/// <summary>
/// Implementation of ICurrentUserService that extracts user information from HTTP context.
/// This service bridges the gap between ASP.NET Core's HTTP context and the Application layer.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the current user's ID from the JWT token (sub claim).
    /// </summary>
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");

    /// <summary>
    /// Gets the current user's role from the JWT token.
    /// Supports both standard ClaimTypes.Role and Supabase's "role" claim.
    /// </summary>
    public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)
        ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("role");

    /// <summary>
    /// Gets the current user's email from the JWT token.
    /// </summary>
    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email)
        ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("email");

    /// <summary>
    /// Checks if the current user is authenticated.
    /// </summary>
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// Checks if the current user has the Admin role.
    /// </summary>
    public bool IsAdmin => Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) ?? false;

    /// <summary>
    /// Checks if the current user has the Seller role.
    /// </summary>
    public bool IsSeller => Role?.Equals("Seller", StringComparison.OrdinalIgnoreCase) ?? false;

    /// <summary>
    /// Checks if the current user has Admin or Seller role.
    /// </summary>
    public bool IsAdminOrSeller => IsAdmin || IsSeller;
}
