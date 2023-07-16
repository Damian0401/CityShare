using CityShare.Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CityShare.Backend.Tests.Common;

internal class Value
{
    public static readonly string String = string.Empty;
    public static readonly bool True = true;
    public static readonly bool False = false;
    public static readonly object? Null = null;
    public static readonly ApplicationUser ApplicationUser = new ApplicationUser();
    public static readonly CancellationToken CancelationToken = new CancellationToken();
    public static readonly IdentityResult IdentityResultSecceeded = IdentityResult.Success;
    public static readonly IdentityResult IdentityResultFailed = IdentityResult.Failed(Array.Empty<IdentityError>());
}
