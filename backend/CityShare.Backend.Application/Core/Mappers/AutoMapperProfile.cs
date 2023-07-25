using AutoMapper;
using CityShare.Backend.Application.Core.Models.Authentication.Register;
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
        CreateMap<RegisterRequestModel, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
    }
}
