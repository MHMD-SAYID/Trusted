using Microsoft.AspNetCore.Identity;
using Trusted.Core.Entities;

namespace Trusted.Infrastructure.DataSeeding;

public static class UserSeeding
{
    public static async Task SeedAsync(
        UserManager<User> userManager)
    {
        const string mediatorEmail = "mediator@trusted.com";
        const string mediatorPassword = "Mediator123!";

        const string adminEmail = "admin@trusted.com";
        const string adminPassword = "Admin123!";

        // Admin
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var adminResult =
                await userManager.CreateAsync(admin, adminPassword);

            if (!adminResult.Succeeded)
            {
                throw new Exception(
                    "Failed to create admin: " +
                    string.Join(", ",
                        adminResult.Errors.Select(e => e.Description)));
            }
        }

        // Mediator
        var mediator =
            await userManager.FindByEmailAsync(mediatorEmail);

        if (mediator == null)
        {
            mediator = new User
            {
                UserName = mediatorEmail,
                Email = mediatorEmail,
                FirstName = "System",
                LastName = "Mediator",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var mediatorResult =
                await userManager.CreateAsync(
                    mediator,
                    mediatorPassword);

            if (!mediatorResult.Succeeded)
            {
                throw new Exception(
                    "Failed to create mediator: " +
                    string.Join(", ",
                        mediatorResult.Errors.Select(e => e.Description)));
            }
        }

        // Roles
        if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            var result =
                await userManager.AddToRoleAsync(admin, "Admin");

            if (!result.Succeeded)
            {
                throw new Exception(
                    "Failed to assign Admin role: " +
                    string.Join(", ",
                        result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(mediator, "Mediator"))
        {
            var result =
                await userManager.AddToRoleAsync(mediator, "Mediator");

            if (!result.Succeeded)
            {
                throw new Exception(
                    "Failed to assign Mediator role: " +
                    string.Join(", ",
                        result.Errors.Select(e => e.Description)));
            }
        }
    }
}