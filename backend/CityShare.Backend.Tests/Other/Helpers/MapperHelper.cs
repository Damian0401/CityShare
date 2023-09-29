using AutoMapper;
using CityShare.Backend.Application.Core.Mappers;

namespace CityShare.Backend.Tests.Other.Helpers;

public static class MapperHelper
{
    public static IMapper GetMapper() =>
        new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>())
            .CreateMapper();
}
