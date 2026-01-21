using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // Seed roles
        string[] roles = { "Admin", "Customer", "Staff" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"✅ Created role: {role}");
            }
        }

        // Seed admin user (optional)
        var adminEmail = "admin@shopxbase.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var admin = new AppUser
            {
                UserName = "admin",
                Email = adminEmail,
                FullName = "System Administrator",
                Occupation = "Admin",
                Address = "System",
                Avatar = "",
                token = "",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(admin, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                Console.WriteLine($"✅ Created admin user: {adminEmail}");
            }
            else
            {
                Console.WriteLine($"❌ Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
