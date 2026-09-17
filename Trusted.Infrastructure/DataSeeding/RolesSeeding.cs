using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities;

namespace Trusted.Infrastructure.DataSeeding
{
    public static class RolesSeeding
    {
        public static async Task SeedAsync(RoleManager<UserRole> roleManager)
        {
            string[]
                roles = { "User","Admin","Mediator"};
            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new UserRole 
                    { 
                            Name = role,
                            IsActive = true,
                            NormalizedName=role.ToUpperInvariant()
                    });
                }
            }
        }
    }
}
