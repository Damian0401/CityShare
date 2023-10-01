﻿using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Events.Commands;
using CityShare.Backend.Application.Events.Queries;
using CityShare.Backend.Domain.Extensions;
using CityShare.Services.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityShare.Services.Api.Endpoints.V1;

public class Events
{
    public static async Task<IResult> Create(
        [FromBody] CreateEventDto createEventDto,
        ClaimsPrincipal claimsPrincipal,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateEventCommand(createEventDto, claimsPrincipal.GetUserId());

        var result = await mediator.Send(
            command, 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> UploadImage(
        [FromForm] IFormFile image,
        [FromRoute] Guid id,
        [FromQuery] bool? shouldBeBlurred,
        ClaimsPrincipal claimsPrincipal,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new UploadEventImageCommand
        {
            Image = image,
            EventId = id,
            UserId = claimsPrincipal.GetUserId(),
            ShouldBeBlurred = shouldBeBlurred,
        };

        var result = await mediator.Send(
            command, 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> GetById(
        [FromRoute] Guid id,
        ClaimsPrincipal claimsPrincipal,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetEventByIdQuery(id, claimsPrincipal.GetUserId()), 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> GetByQuery(
        [AsParameters] EventSearchQueryDto Request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetEventsByQuery(Request), cancellationToken);

        return ResultResolver.Resolve(result);
    }
}
