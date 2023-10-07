using CityShare.Backend.Application.Core.Dtos.Categories;
using CityShare.Backend.Application.Core.Dtos.Cities;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace CityShare.Backend.Tests.Other.Common;

internal class Value
{
    public static string String => Path.GetRandomFileName();
    public static int Int => Random.Shared.Next();
    public static double Double => Random.Shared.NextDouble();
    public static Guid Guid => Guid.NewGuid();
    public static readonly int Zero = 0;
    public static readonly int One = 1;
    public static readonly bool True = true;
    public static readonly bool False = false;
    public static readonly object? Null = null;
    public static readonly ApplicationUser ApplicationUser = new ApplicationUser();
    public static readonly CancellationToken CancelationToken = new CancellationToken();
    public static readonly IdentityResult IdentityResultSecceeded = IdentityResult.Success;
    public static readonly IdentityResult IdentityResultFailed = IdentityResult.Failed(Array.Empty<IdentityError>());
    public static readonly string JsonEmptyArray = "[]";
    public static readonly string SerializedNull = JsonSerializer.Serialize((object?)null);
    public static readonly string SerializedArrayWithSearchResult = JsonSerializer.Serialize(new[] 
    {
        new MapSearchResponseDto()
    });
    public static readonly string SerializedReverseResponseDto = JsonSerializer.Serialize(new MapReverseResponseDto());
    public static readonly AddressDetailsDto AddressDetailsDto = new AddressDetailsDto();
    public static readonly AddressDto AddressDto = new AddressDto();
    public static readonly Email Email = new Email();
    public static readonly SearchEventDto SearchEventDto = new SearchEventDto
    {
        Event = new Event()
    };
    public static readonly MapSearchResponseDto NominatimSearchResponseDto = new MapSearchResponseDto();
    public static readonly MapReverseResponseDto NominatimReverseResponseDto = new MapReverseResponseDto();
    public static readonly IEnumerable<Category> Categories = Enumerable.Empty<Category>();
    public static readonly IEnumerable<CategoryDto> CategoryDtos = Enumerable.Empty<CategoryDto>();
    public static readonly IEnumerable<City> Cities = Enumerable.Empty<City>();
    public static readonly IEnumerable<CityDto> CityDtos = Enumerable.Empty<CityDto>();
    public static readonly IEnumerable<SearchEventDto> SearchEventDtos = Enumerable.Empty<SearchEventDto>();
}
