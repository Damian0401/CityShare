using CityShare.Backend.Domain.Shared;

namespace CityShare.Backend.Tests.Other.Helpers;

internal class AssertHelper
{
    public static void FailureWithErrors<T>(Result<T> result, IEnumerable<Error> errors)
    {
        var isCorrectStatusCode = result.IsFailure
            && result.Errors is not null
            && errors.All(x => result.Errors.Contains(x));

        Assert.True(isCorrectStatusCode);
    }

    public static void FailureWithErrors(Result result, IEnumerable<Error> errors)
    {
        var isCorrectStatusCode = result.IsFailure
            && result.Errors is not null
            && errors.All(x => result.Errors.Contains(x));

        Assert.True(isCorrectStatusCode);
    }
}
