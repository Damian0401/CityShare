using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Contracts.Authentication.Login;
using CityShare.Backend.Application.Core.Contracts.Authentication.Register;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IJwtProvider jwtProvider,
        IMapper mapper)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager
            .FindByEmailAsync(request.Request.Email);

        if (user is null)
        {
            return Result<LoginResponse>.Failure(Errors.InvalidCredentials);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Request.Password);

        if (!isPasswordValid)
        {
            return Result<LoginResponse>.Failure(Errors.InvalidCredentials);
        }

        var response = await CreateResponseAsync(user);

        return response;
    }

    private async Task<LoginResponse> CreateResponseAsync(ApplicationUser user)
    {
        var accessToken = _jwtProvider.GenerateToken(user);

        var refreshToken = await _userManager.GenerateUserTokenAsync(
            user, RefreshToken.Provider, RefreshToken.Purpose);

        await _userManager.SetAuthenticationTokenAsync(
            user, RefreshToken.Provider, RefreshToken.Name, refreshToken);

        var userDto = _mapper.Map<UserDto>(user);
        userDto.AccessToken = accessToken;

        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
        };

        return new LoginResponse
        {
            User = userDto,
            RefreshToken = refreshToken,
            CookieOptions = options
        };
    }
}
