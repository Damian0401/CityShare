using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using System.Net;

namespace CityShare.Services.Api.Common;

public class ResultResolver
{
    public static IResult Resolve(Result result)
    {
        if (result.IsSuccess)
        {
            return Microsoft.AspNetCore.Http.Results.NoContent();
        }

        if (result.Errors is not null && result.Errors.All(e => e.Equals(Errors.NotFound.First())))
        {
            return Results.NotFound();
        }

        if (result.Errors is not null && result.Errors.All(e => e.Equals(Errors.EmailAlreadyConfirmed.First())))
        {
            return Results.StatusCode(StatusCodes.Status403Forbidden);
        }

        return Results.BadRequest(result.Errors);
    }

    public static IResult Resolve<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        if (result.Errors is not null && result.Errors.All(e => e.Equals(Errors.NotFound.First())))
        {
            return Results.NotFound();
        }

        if (result.Errors is not null && result.Errors.All(e => e.Equals(Errors.EmailAlreadyConfirmed.First())))
        {
            return Results.StatusCode(StatusCodes.Status403Forbidden);
        }

        return Results.BadRequest(result.Errors);
    }
}
