using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using CityShare.Backend.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Contracts.Authentication.Register;

namespace CityShare.Backend.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public RegisterCommandHandler(
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

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager
            .FindByEmailAsync(request.Request.Email);

        if (user is not null)
        {
            return Result<RegisterResponse>.Failure(Errors.EmailTaken);
        }

        user = _mapper.Map<ApplicationUser>(request.Request);

        var result = await _userManager.CreateAsync(user, request.Request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => new Error(x.Code, x.Description))
                .ToList();

            return Result<RegisterResponse>
                .Failure(errors);
        }

        await _userManager.AddToRoleAsync(user, Roles.User);

        var response = await CreateResponseAsync(user);

        return response;
    }

    private async Task<RegisterResponse> CreateResponseAsync(ApplicationUser user)
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

        return new RegisterResponse
        {
            User = userDto,
            RefreshToken = refreshToken,
            CookieOptions = options
        };
    }
}
