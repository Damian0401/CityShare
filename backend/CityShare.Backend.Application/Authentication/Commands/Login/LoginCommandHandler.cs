using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Contracts.Authentication.Login;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IJwtProvider jwtProvider,
        IMapper mapper,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for user with {@Email}", request.Request.Email);

        var user = await _userManager
            .FindByEmailAsync(request.Request.Email);

        if (user is null)
        {
            return Result<LoginResponse>.Failure(Errors.InvalidCredentials);
        }

        _logger.LogInformation("Checking provided password for {@Email}", user.Email);

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
        _logger.LogInformation("Generating tokens for {@Emaill}", user.Email);

        var accessToken = _jwtProvider.GenerateToken(user);

        var refreshToken = await _userManager.GenerateUserTokenAsync(
            user, RefreshToken.Provider, RefreshToken.Purpose);

        _logger.LogInformation("Saving refresh token for {@Email} to database", user.Email);

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
