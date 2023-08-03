using CityShare.Backend.Application.Map.Queries.Reverse;
using CityShare.Backend.Application.Map.Queries.Search;
using MediatR;

namespace CityShare.Backend.Api.Api.V1;

public class Map
{
    public static async Task<IResult> Search(string query, IMediator mediator)
    {
        var result = await mediator.Send(new SearchQuery(query));

        return ResultResolver.Resolve(result);
    }

    public static async  Task<IResult> Reverse(double x, double y, IMediator mediator)
    {
        var result = await mediator.Send(new ReverseQuery(x, y));

        return ResultResolver.Resolve(result);
    }
}
