using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace CityShare.Backend.Tests.Common;

internal class Value
{
    public static string String => Path.GetRandomFileName();
    public static readonly bool True = true;
    public static readonly bool False = false;
    public static readonly object? Null = null;
    public static readonly ApplicationUser ApplicationUser = new ApplicationUser();
    public static readonly CancellationToken CancelationToken = new CancellationToken();
    public static readonly IdentityResult IdentityResultSecceeded = IdentityResult.Success;
    public static readonly IdentityResult IdentityResultFailed = IdentityResult.Failed(Array.Empty<IdentityError>());
    public static readonly string JsonEmptyArray = "[]";
    public static readonly string SerializedArrayWithSearchResult = JsonSerializer.Serialize(new[]
    {
        new SearchResultModel
        {
            display_name = String,
            lat = Random.Shared.NextDouble().ToString(),
            lon = Random.Shared.NextDouble().ToString(),
        }
    });
}
