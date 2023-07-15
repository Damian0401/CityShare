using AutoMapper;
using CityShare.Backend.Application.Core.Contracts.Authentication;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        MapsForUser();
    }

    private void MapsForUser()
    {
        CreateMap<RegisterRequest, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
    }
}
