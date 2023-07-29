using System.Collections.Generic;
using CityShare.Backend.Domain.Shared;

namespace CityShare.Backend.Domain.Constants;

public static class Errors
{
    public const string InternalServerErrorMessage = "Something went wring";

    public static IEnumerable<Error> EmailTaken => new[]
    {
        new Error("Email.Taken", "Email is already taken")
    };
    
    public static IEnumerable<Error> InvalidCredentials => new[]
    {
        new Error("Invalid.Credentials", "Provided credentials are invalid")
    };
}
