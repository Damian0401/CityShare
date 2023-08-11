using CityShare.Backend.Domain.Shared;

namespace CityShare.Backend.Domain.Constants;

public static class Errors
{
    public const string InternalServerErrorMessage = "Something went wring";

    public static IEnumerable<Error> EmailTaken => new[]
    {
        new Error("EmailTaken", "Email is already taken")
    };
    
    public static IEnumerable<Error> InvalidCredentials => new[]
    {
        new Error("InvalidCredentials", "Provided credentials are invalid")
    };

    public static IEnumerable<Error> NotFound => new[]
    {
        new Error("NotFound", "Resource not found")
    };
    
    public static IEnumerable<Error> InvalidToken => new[]
    {
        new Error("InvalidToken", "Provided token is invalid or expired")
    };
}
