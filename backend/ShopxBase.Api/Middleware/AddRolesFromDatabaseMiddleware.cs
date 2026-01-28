using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Api.Middleware;

/// <summary>
/// Middleware to add roles from ASP.NET Core Identity to HttpContext.User claims
/// This allows role-based authorization to work with Supabase JWT tokens
/// </summary>
public class AddRolesFromDatabaseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AddRolesFromDatabaseMiddleware> _logger;

    public AddRolesFromDatabaseMiddleware(RequestDelegate next, ILogger<AddRolesFromDatabaseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<AppUser> userManager)
    {
        // Only process authenticated requests
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            try
            {
                // Extract email from claims (Supabase uses "email" claim)
                var email = context.User.FindFirst(ClaimTypes.Email)?.Value
                    ?? context.User.FindFirst("email")?.Value;

                if (!string.IsNullOrEmpty(email))
                {
                    // Find user in AppUser table
                    var appUser = await userManager.FindByEmailAsync(email);

                    if (appUser != null)
                    {
                        // Get roles from AspNetUserRoles
                        var roles = await userManager.GetRolesAsync(appUser);

                        if (roles.Any())
                        {
                            // Create new claims list with original claims + role claims
                            var claims = new List<Claim>(context.User.Claims);

                            // Add each role as a claim
                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }

                            // Create new ClaimsIdentity with updated claims
                            var identity = new ClaimsIdentity(claims, context.User.Identity.AuthenticationType);
                            context.User = new ClaimsPrincipal(identity);

                            _logger.LogDebug("Added {RoleCount} roles to user {Email}", roles.Count, email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding roles from database to user claims");
                // Continue without roles if error occurs - don't break the request
            }
        }

        await _next(context);
    }
}
