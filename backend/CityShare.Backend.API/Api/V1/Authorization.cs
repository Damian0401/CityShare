﻿using Azure.Core;
using CityShare.Backend.Application.Authentication.Commands.Register;
using CityShare.Backend.Application.Core.Contracts.Authentication;
using CityShare.Backend.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace CityShare.Backend.Api.Api.V1;

public class Authorization
{
    public static async Task<IResult> Register(
        [FromBody] RegisterRequest request, HttpResponse response, IMediator mediator)
    {
        var result = await mediator.Send(new RegisterCommand(request));

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Errors);
        }

        (var user, var refreshToken, var cookieOptions) = result.Value!;

        response.Cookies.Append(RefreshToken.CookieKey, refreshToken, cookieOptions);

        return Results.Ok(user);
    }
}
