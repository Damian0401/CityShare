using AutoMapper;
using CityShare.Backend.Application.Core.Models.Authentication.Register;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using System.Globalization;
using CityShare.Backend.Application.Core.Models.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Models.Map.Reverse;
using CityShare.Backend.Application.Core.Models.Map.Search;

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
        CreateMap<RegisterRequestModel, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
    }

    private void MapsForNominatim()
    {
        CreateMap<Models.Nominatim.Search.NominatimSearchResponseModel, MapSearchResponseModel>()
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
        CreateMap<Models.Nominatim.Reverse.NominatimReverseResponseModel, MapReverseResponseModel>()
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
