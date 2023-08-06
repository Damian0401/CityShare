using Microsoft.AspNetCore.Identity;

namespace CityShare.Backend.Persistence.Data;

internal static class Roles
{
    internal static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = typeof(Domain.Constants.Roles)
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
}
