using CityShare.Backend.Application.Core.Dtos.Requests;
using CityShare.Backend.Application.Requests.Commands;
using CityShare.Backend.Application.Requests.Queries;
using CityShare.Backend.Domain.Extensions;
using CityShare.Services.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CityShare.Services.Api.Endpoints.V1;

public class Requests
{
    public static async Task<IResult> Create(
        [FromBody] CreateRequestDto dto,
        ClaimsPrincipal claimsPrincipal,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateRequestCommand(dto, claimsPrincipal.GetUserId());

        var result = await mediator.Send(
            command, 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> GetTypes(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new GetRequestTypesQuery();

        var result = await mediator.Send(
            query,
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> Accept(
        [FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new AcceptRequestCommand(id);

        var result = await mediator.Send(
            command,
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> Reject(
        [FromRoute] Guid id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new RejectRequestCommand(id);

        var result = await mediator.Send(
            command,
            cancellationToken);

        return ResultResolver.Resolve(result);
    }
}
