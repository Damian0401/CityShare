﻿using AutoMapper;
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
        MapsForUser();
        MapsForSearch();
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
            .ForMember(x => x.X, s => s.MapFrom(l => double.Parse(l.lat, CultureInfo.InvariantCulture)))
            .ForMember(x => x.Y, s => s.MapFrom(l => double.Parse(l.lon, CultureInfo.InvariantCulture)))
            .ForMember(x => x.BoundingBox, s => s.MapFrom(b => 
                new BoundingBox(
                    double.Parse(b.boundingbox[0], CultureInfo.InvariantCulture),
                    double.Parse(b.boundingbox[1], CultureInfo.InvariantCulture),
                    double.Parse(b.boundingbox[2], CultureInfo.InvariantCulture), 
                    double.Parse(b.boundingbox[3], CultureInfo.InvariantCulture))));
    }
}
