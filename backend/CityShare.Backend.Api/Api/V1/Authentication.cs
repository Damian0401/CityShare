using CityShare.Backend.Application.Authentication.Commands.Login;
using CityShare.Backend.Application.Authentication.Commands.Refresh;
using CityShare.Backend.Application.Authentication.Commands.Register;
using CityShare.Backend.Application.Core.Models.Authentication.Login;
using CityShare.Backend.Application.Core.Models.Authentication.Refresh;
using CityShare.Backend.Application.Core.Models.Authentication.Register;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Api.Api.V1;

public class Authentication
{
    public static async Task<IResult> Register(
        [FromBody] RegisterRequestModel request,
        HttpResponse response, 
        IOptions<JwtSettings> jwtSettings,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RegisterCommand(request), cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Errors);
        }

        (var user, var refreshToken) = result.Value;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationDays),
            Secure = true,
            SameSite = SameSiteMode.None
        };

        response.Cookies.Append(RefreshToken.CookieKey, refreshToken, cookieOptions);

        return Results.Ok(user);
    }
    
    public static async Task<IResult> Login(
        [FromBody] LoginRequestModel request, 
        HttpResponse response,
        IOptions<JwtSettings> jwtSettings,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LoginCommand(request), cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Errors);
        }

        (var user, var refreshToken) = result.Value;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTimeOffset.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationDays),
            SameSite = SameSiteMode.None
        };

        response.Cookies.Append(RefreshToken.CookieKey, refreshToken, cookieOptions);

        return Results.Ok(user);
    }

    public static async Task<IResult> Refresh(
        [FromBody] RefreshRequestModel refreshRequest,
        HttpRequest request, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var refreshToken = request.Cookies[RefreshToken.CookieKey];

        if (refreshToken is null)
        {
            return Results.Unauthorized();
        }

        var result = await mediator.Send(new RefreshCommand(refreshRequest, refreshToken), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value.User) 
            : Results.Unauthorized();
    }
}
