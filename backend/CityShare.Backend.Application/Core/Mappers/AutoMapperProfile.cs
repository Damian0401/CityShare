﻿using AutoMapper;
using CityShare.Backend.Domain.Entities;
using System.Globalization;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Application.Core.Dtos.Cities;
using CityShare.Backend.Application.Core.Dtos.Categories;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Events.Commands;
using CityShare.Backend.Application.Core.Dtos.Comments;
using CityShare.Backend.Application.Core.Dtos.Requests;
using CityShare.Backend.Application.Core.Dtos.Images;

namespace CityShare.Backend.Application.Core.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        MapsForUser();
        MapsForMaps();
        MapsForEmails();
        MapsForAddresses();
        MapsForCities();
        MapsForCategories();
        MapsForEvents();
        MapsForImages();
        MapsForLikes();
        MapsForComments();
        MapsForRequests();
    }

    private void MapsForUser()
    {
        CreateMap<RegisterDto, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
    }

    private void MapsForMaps()
    {
        CreateMap<MapSearchResponseDto, AddressDetailsDto>()
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
        CreateMap<MapReverseResponseDto, AddressDto>()
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

    private void MapsForAddresses()
    {
        CreateMap<BoundingBox, BoundingBoxDto>();
        CreateMap<Address, AddressDto>()
            .ForMember(x => x.Point, s => s.MapFrom(a => new PointDto(a.X, a.Y)));
        CreateMap<Address, AddressDetailsDto>()
            .ForMember(x => x.Point, s => s.MapFrom(a => new PointDto(a.X, a.Y)));
        CreateMap<AddressDto, Address>()
            .ForMember(x => x.X, s => s.MapFrom(a => a.Point.X))
            .ForMember(x => x.Y, s => s.MapFrom(a => a.Point.Y));
    }

    private void MapsForCities()
    {
        CreateMap<City, CityDto>()
            .ForPath(x => x.Address, s => s.MapFrom(a => a.Address))
            .ForPath(x => x.Address.BoundingBox, s => s.MapFrom(a => a.BoundingBox));
    }

    private void MapsForCategories()
    {
        CreateMap<Category, CategoryDto>();
    }

    private void MapsForEvents()
    {
        CreateMap<CreateEventDto, Event>()
            .ForMember(x => x.Address, s => s.MapFrom(e => e.Address));
        CreateMap<Event, EventDto>()
            .ForMember(x => x.CategoryIds, s => s.MapFrom(e => e.EventCategories.Select(c => c.CategoryId)));
    }

    private void MapsForLikes()
    {
        CreateMap<UpdateEventLikesCommand, Like>();
    }

    private void MapsForComments()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(x => x.Author, s => s.MapFrom(c => c.Author.UserName));
    }

    private void MapsForRequests()
    {
        CreateMap<CreateRequestDto, Request>();
        CreateMap<RequestType, RequestTypeDto>();
        CreateMap<Request,  RequestDto>()
            .ForMember(x => x.EventId, s => s.MapFrom(r => r.Image!.EventId));
    }

    private void MapsForImages()
    {
        CreateMap<Image, ImageDto>()
            .ForMember(x => x.Uri, s => s.MapFrom(i => i.IsBlurred == i.ShouldBeBlurred ? i.Uri : null));
    }
}
