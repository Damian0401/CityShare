using AutoMapper;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Entities;
using System.Globalization;
using CityShare.Backend.Application.Core.Dtos.Authentication.Register;
using CityShare.Backend.Application.Core.Dtos.Authentication;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Search;
using CityShare.Backend.Application.Core.Dtos.Map;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Reverse;

namespace CityShare.Backend.Application.Core.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        MapsForUser();
        MapsForNominatim();
        MapsForEmails();
    }

    private void MapsForUser()
    {
        CreateMap<RegisterRequestDto, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
    }

    private void MapsForNominatim()
    {
        CreateMap<NominatimSearchResponseDto, AddressDetailsDto>()
            .ForMember(x => x.DisplayName, s => s.MapFrom(d => d.display_name.Replace("\"", "'")))
            .ForMember(x => x.Point, s => s.MapFrom(l => new PointDto(
                double.Parse(l.lat, CultureInfo.InvariantCulture),
                double.Parse(l.lon, CultureInfo.InvariantCulture))))
            .ForMember(x => x.BoundingBox, s => s.MapFrom(b => 
                new BoundingBoxDto(
                    double.Parse(b.boundingbox[0], CultureInfo.InvariantCulture),
                    double.Parse(b.boundingbox[1], CultureInfo.InvariantCulture),
                    double.Parse(b.boundingbox[2], CultureInfo.InvariantCulture), 
                    double.Parse(b.boundingbox[3], CultureInfo.InvariantCulture))));
        CreateMap<NominatimReverseResponseDto, AddressDto>()
            .ForMember(x => x.DisplayName, s => s.MapFrom(d => d.display_name.Replace("\"", "'")))
            .ForMember(x => x.Point, s => s.MapFrom(l => new PointDto(
                double.Parse(l.lat, CultureInfo.InvariantCulture),
                double.Parse(l.lon, CultureInfo.InvariantCulture))));
    }

    private void MapsForEmails()
    {
        CreateMap<EmailTemplate, Email>()
            .ForMember(x => x.Id, s => s.Ignore());
    }
}
