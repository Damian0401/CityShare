using CityShare.Backend.Application.Auth.Commands;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Application.Core.Dtos.Auth.ConfirmEmail;
using CityShare.Backend.Application.Core.Dtos.Auth.Login;
using CityShare.Backend.Application.Core.Dtos.Auth.Refresh;
using CityShare.Backend.Application.Core.Dtos.Auth.Register;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Settings;
using CityShare.Services.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CityShare.Services.Api.Endpoints.V1;

public class Auth
{
    public static async Task<IResult> Register(
        [FromBody] RegisterRequestDto request,
        HttpResponse response, 
        IOptions<AuthSettings> authSettings,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new RegisterCommand(request), 
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
            new LoginCommand(request), 
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
            new RefreshCommand(refreshRequest, refreshToken), 
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
            new ConfirmEmailCommand(request), 
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
