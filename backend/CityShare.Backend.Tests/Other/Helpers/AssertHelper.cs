﻿using CityShare.Backend.Domain.Shared;

namespace CityShare.Backend.Tests.Other.Helpers;

internal class AssertHelper
{
    public static void FailureWithStatusCode<T>(Result<T> result, IEnumerable<Error> errors)
    {
        var isCorrectStatusCode = result.IsFailure
            && result.Errors is not null
            && errors.All(x => result.Errors.Contains(x));

        Assert.True(isCorrectStatusCode);
    }

    public static void FailureWithStatusCode(Result result, IEnumerable<Error> errors)
    {
        var isCorrectStatusCode = result.IsFailure
            && result.Errors is not null
            && errors.All(x => result.Errors.Contains(x));

        Assert.True(isCorrectStatusCode);
    }
}
