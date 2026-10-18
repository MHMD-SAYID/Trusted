using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trusted.Core.Entities;

namespace Trusted.Infrastructure.DataSeeding;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseSeeder");

        try
        {
            var roleManager = services.GetRequiredService<RoleManager<UserRole>>();
            await RolesSeeding.SeedAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<User>>();
            await UserSeeding.SeedAsync(userManager);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Roles/users seeding failed.");
        }

        try
        {
            var db = services.GetRequiredService<ApplicationDbContext>();
            await gamesSeeding.SeedAsync(db);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Games seeding failed.");
            throw;
        }
    }
}
