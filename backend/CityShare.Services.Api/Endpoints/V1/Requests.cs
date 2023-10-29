using CityShare.Backend.Application.Core.Dtos.Requests;
using CityShare.Backend.Application.Requests.Commands;
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
}
