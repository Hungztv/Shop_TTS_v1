using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "Customer", "Seller" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
