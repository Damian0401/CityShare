using CityShare.Backend.Application.Map.Queries;
using CityShare.Services.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CityShare.Services.Api.Api.V1;

public class Map
{
    public static async Task<IResult> Search(
        [FromQuery] string query,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new SearchQuery(query), 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> Reverse(
        [FromQuery] double x,
        [FromQuery] double y, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ReverseQuery(x, y), 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }
}
