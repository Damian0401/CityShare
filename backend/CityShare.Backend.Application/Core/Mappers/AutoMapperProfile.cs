using AutoMapper;
using CityShare.Backend.Application.Core.Models.Authentication.Register;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using System.Globalization;

namespace CityShare.Backend.Application.Core.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        MapsForTypes();
        MapsForUser();
        MapsForSearch();
    }

    private void MapsForTypes()
    {
        CreateMap<string, double>()
            .ConvertUsing(s => double.Parse(s, CultureInfo.InvariantCulture));
    }

    private void MapsForUser()
    {
        CreateMap<RegisterRequestModel, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
    }

    private void MapsForSearch()
    {
        CreateMap<SearchResultModel, SearchDto>()
            .ForMember(x => x.DisplayName, s => s.MapFrom(d => d.display_name))
            .ForMember(x => x.X, s => s.MapFrom(l => l.lat))
            .ForMember(x => x.Y, s => s.MapFrom(l => l.lon));
    }
}
