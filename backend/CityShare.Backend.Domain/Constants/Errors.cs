﻿using CityShare.Backend.Domain.Shared;

namespace CityShare.Backend.Domain.Constants;

public static class Errors
{
    public const string InternalServerErrorMessage = "Something went wrong";

    public static IEnumerable<Error> EmailTaken(string email) => new[]
    {
        new Error("EmailTaken", $"Email {email} is already taken")
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
    
    public static IEnumerable<Error> ForbiddenState => new[]
    {
        new Error("ForbiddenState", "Forbidden state has occurred")
    };

    public static IEnumerable<Error> OperationFailed => new[]
    {
        new Error("OperationFailed", "Unable to perform requested operation")
    };

    public static IEnumerable<Error> EmailAlreadyConfirmed => new[]
    {
        new Error("EmailAlreadyConfirmed", "Email is already confirmed")
    };
}
