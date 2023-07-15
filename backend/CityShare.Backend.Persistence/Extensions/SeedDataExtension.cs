using CityShare.Backend.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CityShare.Backend.Persistence.Extensions;

public static class SeedDataExtension
{
    public static async Task<IServiceProvider> SeedDataAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var services = scope.ServiceProvider;

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await SeedRoles(roleManager);
        
        return serviceProvider;
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (roleManager.Roles.Any())
        {
            return;
        }

        var roles = typeof(Roles)
            .GetFields()
            .Select(x => x.GetValue(null))
            .Cast<string>();

        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (roleExists)
            {
                continue;
            }

            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
