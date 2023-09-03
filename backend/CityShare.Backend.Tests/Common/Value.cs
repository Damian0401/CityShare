using CityShare.Backend.Application.Core.Dtos.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Search;
using CityShare.Backend.Domain.Entities;
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
        new NominatimSearchResponseDto()
    });
    public static readonly string SerializedReverseResponseDto = JsonSerializer.Serialize(new NominatimReverseResponseDto());
    public static NominatimSearchResponseDto NominatimSearchResponseDto => new NominatimSearchResponseDto();
    public static NominatimReverseResponseDto NominatimReverseResponseDto => new NominatimReverseResponseDto();
    public static Email Email => new Email();
    public static EmailPriority EmailPriority => new EmailPriority();
}
