using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Models.Map.Reverse;
using CityShare.Backend.Application.Core.Models.Map.Search;
using CityShare.Backend.Application.Core.Models.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace CityShare.Backend.Tests.Common;

internal class Value
{
    public static string String => Path.GetRandomFileName();
    public static int Int => Random.Shared.Next();
    public static double Double => Random.Shared.NextDouble();
    public static readonly bool True = true;
    public static readonly bool False = false;
    public static readonly object? Null = null;
    public static readonly Guid Guid = Guid.NewGuid();
    public static readonly ApplicationUser ApplicationUser = new ApplicationUser();
    public static readonly CancellationToken CancelationToken = new CancellationToken();
    public static readonly IdentityResult IdentityResultSecceeded = IdentityResult.Success;
    public static readonly IdentityResult IdentityResultFailed = IdentityResult.Failed(Array.Empty<IdentityError>());
    public static readonly string JsonEmptyArray = "[]";
    public static readonly string SerializedNull = JsonSerializer.Serialize((object?)null);
    public static readonly string SerializedArrayWithSearchResult = JsonSerializer.Serialize(new[] 
    {
        new NominatimSearchResponseModel()
    });
    public static readonly string SerializedReverseResult = JsonSerializer.Serialize(new Application.Core.Models.Nominatim.Reverse.NominatimReverseResponseModel());
    public static MapSearchResponseModel MapSearchResponseModel => new MapSearchResponseModel();
    public static MapReverseResponseModel MapReverseResponseModel => new MapReverseResponseModel();
    public static Email Email => new Email();
    public static EmailPriority EmailPriority => new EmailPriority();
}
