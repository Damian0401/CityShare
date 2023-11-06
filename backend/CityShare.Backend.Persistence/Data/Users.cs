using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CityShare.Backend.Persistence.Data;

internal static class Users
{
    internal static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = typeof(Roles)
            .GetFields()
            .Select(x => x.GetValue(null))
            .Cast<string>();

        var existingRoles = roleManager.Roles
            .Select(x => x.Name);

        foreach (var role in roles)
        {
            var roleExists = existingRoles.Contains(role);

            if (roleExists)
            {
                continue;
            }

            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    internal static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, EmailSettings emailSettings, IConfiguration configuration)
    {
        if (userManager.Users.Any())
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = AdminAccount.Login,
            Email = emailSettings.Address,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, configuration[AdminAccount.PasswordKey] ?? AdminAccount.DefaultPassword);

        await userManager.AddToRoleAsync(user, Roles.Admin);
    }
}
