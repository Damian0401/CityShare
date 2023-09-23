using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Events.Commands;
using CityShare.Backend.Domain.Extensions;
using CityShare.Services.Api.Common;
using MediatR;
using System.Security.Claims;

namespace CityShare.Services.Api.Endpoints.V1;

public class Events
{
    public static async Task<IResult> Create(
        CreateEventDto createEventDto,
        ClaimsPrincipal claimsPrincipal,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateEventCommand(createEventDto, claimsPrincipal.GetUserId());

        var result = await mediator.Send(command, cancellationToken);

        return ResultResolver.Resolve(result);
    }
}
