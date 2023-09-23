using CityShare.Backend.Application.Cities.Queries;
using CityShare.Services.Api.Common;
using MediatR;

namespace CityShare.Services.Api.Endpoints.V1;

public class Cities
{
    public static async Task<IResult> GetAll(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllCitiesQuery(), cancellationToken);

        return ResultResolver.Resolve(result);
    }
}
