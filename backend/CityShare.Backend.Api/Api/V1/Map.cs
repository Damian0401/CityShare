using CityShare.Backend.Api.Common;
using CityShare.Backend.Application.Map.Queries.Reverse;
using CityShare.Backend.Application.Map.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CityShare.Backend.Api.Api.V1;

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
