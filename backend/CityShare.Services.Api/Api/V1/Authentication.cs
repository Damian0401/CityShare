﻿using CityShare.Backend.Application.Authentication.Commands;
using CityShare.Backend.Application.Core.Dtos.Authentication;
using CityShare.Backend.Application.Core.Dtos.Authentication.ConfirmEmail;
using CityShare.Backend.Application.Core.Dtos.Authentication.Login;
using CityShare.Backend.Application.Core.Dtos.Authentication.Refresh;
using CityShare.Backend.Application.Core.Dtos.Authentication.Register;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Settings;
using CityShare.Services.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CityShare.Services.Api.Api.V1;

public class Authentication
{
    public static async Task<IResult> Register(
        [FromBody] RegisterRequestDto request,
        HttpResponse response, 
        IOptions<AuthSettings> authSettings,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new Register.Command(request), 
            cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Errors);
        }

        return ReturnResponseAndToken(
            response, 
            authSettings, 
            result.Value.User, 
            result.Value.RefreshToken);
    }

    public static async Task<IResult> Login(
        [FromBody] LoginRequestDto request, 
        HttpResponse response,
        IOptions<AuthSettings> authSettings,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new Login.Command(request), 
            cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Errors);
        }

        return ReturnResponseAndToken(
            response,
            authSettings,
            result.Value.User,
            result.Value.RefreshToken);
    }

    public static async Task<IResult> Refresh(
        [FromBody] RefreshRequestDto refreshRequest,
        HttpRequest request, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var refreshToken = request.Cookies[RefreshToken.CookieKey];

        if (refreshToken is null)
        {
            return Results.Unauthorized();
        }

        var result = await mediator.Send(
            new Refresh.Command(refreshRequest, refreshToken), 
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value.User) 
            : Results.Unauthorized();
    }

    public static async Task<IResult> ConfirmEmail(
        [FromBody] EmailConfirmRequestDto request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ConfirmEmail.Command(request), 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    private static IResult ReturnResponseAndToken(
        HttpResponse response, 
        IOptions<AuthSettings> authSettings, 
        UserDto user, 
        string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow
                .AddDays(authSettings.Value.RefreshTokenExpirationDays),
            Secure = true,
            SameSite = SameSiteMode.None
        };

        response.Cookies.Append(
            RefreshToken.CookieKey, 
            refreshToken, 
            cookieOptions);

        return Results.Ok(user);
    }
}
