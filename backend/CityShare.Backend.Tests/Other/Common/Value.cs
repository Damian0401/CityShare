using CityShare.Backend.Application.Core.Dtos.Categories;
using CityShare.Backend.Application.Core.Dtos.Cities;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Core.Dtos.Map;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Search;
using CityShare.Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace CityShare.Backend.Tests.Other.Common;

internal class Value
{
    public static string String => Path.GetRandomFileName();
    public static int Int => Random.Shared.Next();
    public static int Zero => 0;
    public static int One => 1;
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
    public static AddressDetailsDto AddressDetailsDto => new AddressDetailsDto();
    public static AddressDto AddressDto => new AddressDto();
    public static Email Email => new Email();
    public static SearchEventDto SearchEventDto => new SearchEventDto
    {
        Event = new Event()
    };
    public static EmailPriority EmailPriority => new EmailPriority();
    public static NominatimSearchResponseDto NominatimSearchResponseDto => new NominatimSearchResponseDto();
    public static NominatimReverseResponseDto NominatimReverseResponseDto => new NominatimReverseResponseDto();
    public static IEnumerable<Category> Categories => Enumerable.Empty<Category>();
    public static IEnumerable<CategoryDto> CategoryDtos => Enumerable.Empty<CategoryDto>();
    public static IEnumerable<City> Cities => Enumerable.Empty<City>();
    public static IEnumerable<CityDto> CityDtos => Enumerable.Empty<CityDto>();
    public static IEnumerable<SearchEventDto> SearchEventDtos => Enumerable.Empty<SearchEventDto>();
}
