using CityShare.Backend.Persistence.Data;
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

        var context = services.GetRequiredService<CityShareDbContext>();

        await Users.SeedRolesAsync(roleManager);

        await Emails.SeedEmailTemplatesAsync(context);

        await Emails.SeedEmailPrioritiesAsync(context);

        await Emails.SeedEmailStatusesAsync(context);

        await Events.SeedCitiesAsync(context);

        await Events.SeedCategoriesAsync(context);

        return serviceProvider;
    }
}
