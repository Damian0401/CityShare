using System.Collections.Generic;
using CityShare.Backend.Domain.Shared;

namespace CityShare.Backend.Domain.Constants;

public static class Errors
{
    public static IEnumerable<Error> EmailAlreadyTaken => new[]
    {
        new Error("Email.Taken", "Email is already taken")
    };
}
