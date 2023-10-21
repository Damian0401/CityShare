using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Persistence.Extensions;

public static class SeedDataExtension
{
    public static async Task<IServiceProvider> SeedDataAsync(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();

        var services = scope.ServiceProvider;

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        var emailSettings = services.GetRequiredService<IOptions<EmailSettings>>().Value;

        var context = services.GetRequiredService<CityShareDbContext>();

        await Users.SeedRolesAsync(roleManager);

        await Users.SeedAdminAsync(userManager, emailSettings, configuration);

        await Emails.SeedEmailTemplatesAsync(context);

        await Emails.SeedEmailStatusesAsync(context);

        await Events.SeedCitiesAsync(context);

        await Events.SeedCategoriesAsync(context);

        return serviceProvider;
    }
}
