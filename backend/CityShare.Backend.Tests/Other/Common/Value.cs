using CityShare.Backend.Application.Core.Dtos.Categories;
using CityShare.Backend.Application.Core.Dtos.Cities;
using CityShare.Backend.Application.Core.Dtos.Comments;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Text.Json;

namespace CityShare.Backend.Tests.Other.Common;

internal class Value
{
    public static readonly int Zero = 0;
    public static readonly int One = 1;
    public static readonly bool True = true;
    public static readonly bool False = false;
    public static readonly object? Null = null;

    public static int Int => Random.Shared.Next();
    public static double Double => Random.Shared.NextDouble();
    public static string String => Path.GetRandomFileName();
    public static Guid Guid => Guid.NewGuid();
    public static string JsonEmptyArray => "[]";
    public static IdentityResult IdentityResultSecceeded => IdentityResult.Success;
    public static string SerializedNull => JsonSerializer.Serialize((object?)null);
    public static string SerializedArrayWithSearchResult => JsonSerializer.Serialize(new[] 
    {
        new MapSearchResponseDto()
    });
    public static ApplicationUser ApplicationUser => new ApplicationUser();
    public static CancellationToken CancelationToken => new CancellationToken();
    public static IdentityResult IdentityResultFailed = IdentityResult.Failed(Array.Empty<IdentityError>());
    public static string SerializedReverseResponseDto => JsonSerializer.Serialize(new MapReverseResponseDto());
    public static AddressDetailsDto AddressDetailsDto => new AddressDetailsDto();
    public static AddressDto AddressDto => new AddressDto();
    public static Email Email => new Email();
    public static Image Image => new Image();
    public static Stream Stream => new Mock<Stream>().Object;
    public static readonly SearchEventDto SearchEventDto = new SearchEventDto
    {
        Event = new Event()
    };
    public static MapSearchResponseDto NominatimSearchResponseDto => new MapSearchResponseDto();
    public static MapReverseResponseDto NominatimReverseResponseDto => new MapReverseResponseDto();
    public static IEnumerable<Category> Categories => Enumerable.Empty<Category>();
    public static IEnumerable<CategoryDto> CategoryDtos => Enumerable.Empty<CategoryDto>();
    public static IEnumerable<City> Cities => Enumerable.Empty<City>();
    public static IEnumerable<CityDto> CityDtos => Enumerable.Empty<CityDto>();
    public static IEnumerable<SearchEventDto> SearchEventDtos => Enumerable.Empty<SearchEventDto>();
    public static CreateCommentDto CreateCommentDto => new CreateCommentDto();
    public static City City => new City();
    public static BoundingBox BoundingBox => new BoundingBox();
    public static PointDto PointDtoAtX0Y0 => new PointDto(0, 0);
}
