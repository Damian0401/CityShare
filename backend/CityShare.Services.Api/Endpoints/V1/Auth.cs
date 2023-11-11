using CityShare.Backend.Application.Auth.Commands;
using CityShare.Backend.Application.Auth.Queries;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Extensions;
using CityShare.Backend.Domain.Settings;
using CityShare.Services.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CityShare.Services.Api.Endpoints.V1;

public class Auth
{
    public static async Task<IResult> Register(
        [FromBody] RegisterDto request,
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
            result.Value);
    }

    public static async Task<IResult> Login(
        [FromBody] LoginDto request, 
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
            result.Value);
    }

    public static async Task<IResult> Refresh(
        [FromBody] RefreshDto refreshRequest,
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

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> ConfirmEmail(
        [FromBody] EmailConfirmDto request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ConfirmEmailCommand(request), 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    public static async Task<IResult> Profile(
        ClaimsPrincipal claimsPrincipal,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ProfileQuery(claimsPrincipal.GetUserId()), 
            cancellationToken);

        return ResultResolver.Resolve(result);
    }

    private static IResult ReturnResponseAndToken(
        HttpResponse response, 
        IOptions<AuthSettings> authSettings, 
        AuthResponseDto dto)
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
            dto.RefreshToken, 
            cookieOptions);

        return Results.Ok(dto.User);
    }
}
