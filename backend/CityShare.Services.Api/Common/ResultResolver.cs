﻿using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;

namespace CityShare.Services.Api.Common;

public class ResultResolver
{
    public static IResult Resolve(Result result)
    {
        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        if (result.Errors is not null &&
            result.Errors.Any(x => x.Code.Equals(Errors.NotFound.First().Code)))
        {
            return Results.NotFound();
        }

        return Results.BadRequest(result.Errors);
    }

    public static IResult Resolve<T>(Result<T> result)
        where T : class
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        if (result.Errors is not null && 
            result.Errors.Any(x => x.Code.Equals(Errors.NotFound.First().Code)))
        {
            return Results.NotFound();
        }

        return Results.BadRequest(result.Errors);
    }
}