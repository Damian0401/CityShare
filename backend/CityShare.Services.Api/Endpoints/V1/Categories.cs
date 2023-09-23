using CityShare.Backend.Application.Categories.Queries;
using CityShare.Services.Api.Common;
using MediatR;

namespace CityShare.Services.Api.Endpoints.V1;

public class Categories
{
    public static async Task<IResult> GetAll(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllCategoriesQuery(), cancellationToken);

        return ResultResolver.Resolve(result);
    }
}
