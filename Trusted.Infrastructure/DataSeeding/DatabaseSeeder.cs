using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities;

namespace Trusted.Infrastructure.DataSeeding
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager =
                services.GetRequiredService<RoleManager<UserRole>>();

            await RolesSeeding.SeedAsync(roleManager);

            var userManager =
                services.GetRequiredService<UserManager<User>>();

            await UserSeeding.SeedAsync(userManager);

            //await LookupSeeding.SeedAsync(services);

            //await PermissionsSeeding.SeedAsync(services);
        }
    }
}
