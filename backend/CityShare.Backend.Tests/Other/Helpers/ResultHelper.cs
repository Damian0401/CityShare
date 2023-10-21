using CityShare.Backend.Domain.Shared;

namespace CityShare.Backend.Tests.Other.Helpers;

internal class ResultHelper
{
    public static bool IsCorrectErrorCode<T>(Result<T> result, IEnumerable<Error> errors)
    {
        return result.IsFailure 
            && result.Errors is not null 
            && errors.All(x => result.Errors.Contains(x));
    }
}
