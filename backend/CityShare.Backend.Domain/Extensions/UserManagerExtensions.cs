using Microsoft.AspNetCore.Identity;

namespace CityShare.Backend.Domain.Extensions;

public static class UserManagerExtensions
{
    public static async Task<bool> IsEmailConfirmedAsync<T>(this UserManager<T> userManager, string userId)
        where T : IdentityUser
    {
        var user = await userManager.FindByIdAsync(userId);

        return user is not null && user.EmailConfirmed;
    }
}
