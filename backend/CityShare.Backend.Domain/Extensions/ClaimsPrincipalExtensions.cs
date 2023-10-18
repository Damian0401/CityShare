using System.Security.Claims;

namespace CityShare.Backend.Domain.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserEmail(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.Email) ?? string.Empty; 
    public static string GetUserId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty; 
    public static string GetUserName(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.Name) ?? string.Empty; 
}
