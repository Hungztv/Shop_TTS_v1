namespace ShopxBase.Application.Interfaces;

/// <summary>
/// Service interface for accessing current authenticated user information.
/// This abstraction allows the Application layer to access user context
/// without depending on ASP.NET Core HTTP context directly.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current authenticated user's ID from the JWT token.
    /// Returns null if the user is not authenticated.
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Gets the current authenticated user's role from the JWT token.
    /// Returns null if the user is not authenticated or has no role.
    /// </summary>
    string? Role { get; }

    /// <summary>
    /// Gets the current authenticated user's email from the JWT token.
    /// Returns null if the user is not authenticated.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Checks if the current user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Checks if the current user has the Admin role.
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// Checks if the current user has the Seller role.
    /// </summary>
    bool IsSeller { get; }

    /// <summary>
    /// Checks if the current user has Admin or Seller role.
    /// </summary>
    bool IsAdminOrSeller { get; }
}
