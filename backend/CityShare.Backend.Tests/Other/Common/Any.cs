using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Dtos.Categories;
using CityShare.Backend.Application.Core.Dtos.Cities;
using CityShare.Backend.Application.Core.Dtos.Comments;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Domain.Entities;
using Moq;

namespace CityShare.Backend.Tests.Other.Common;

internal class Any
{
    public static object Object => It.IsAny<object>();
    public static int Int => It.IsAny<int>();
    public static double Double => It.IsAny<double>();
    public static string String => It.IsAny<string>();
    public static Guid Guid => It.IsAny<Guid>();
    public static Email Email => It.IsAny<Email>();
    public static ApplicationUser ApplicationUser => It.IsAny<ApplicationUser>();
    public static CancellationToken CancellationToken => It.IsAny<CancellationToken>();
    public static MapSearchRequestDto NominatimSearchRequestDto => It.IsAny<MapSearchRequestDto>();
    public static AddressDetailsDto AddressDetailsDto => It.IsAny<AddressDetailsDto>();
    public static CacheServiceOptions CacheServiceOptions => It.IsAny<CacheServiceOptions>();
    public static AddressDto AddressDto => It.IsAny<AddressDto>();
    public static IEnumerable<CategoryDto> CategoryDtos => It.IsAny<IEnumerable<CategoryDto>>();
    public static IEnumerable<CityDto> CityDtos => It.IsAny<IEnumerable<CityDto>>();
    public static EventSearchQueryDto EventQueryDto => It.IsAny<EventSearchQueryDto>();
    public static Like Like => It.IsAny<Like>();
    public static CommentDto CommentDto => It.IsAny<CommentDto>();
}
