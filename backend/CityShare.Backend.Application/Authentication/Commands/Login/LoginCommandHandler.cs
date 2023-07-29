using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Models.Authentication.Login;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtProvider jwtProvider,
        IMapper mapper,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<LoginResponseModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for user with {@Email}", request.Request.Email);

        var user = await _userManager
            .FindByEmailAsync(request.Request.Email);

        if (user is null)
        {
            _logger.LogError("User with {@Email} not found", request.Request.Email);
            return Result<LoginResponseModel>.Failure(Errors.InvalidCredentials);
        }

        _logger.LogInformation("Checking provided password for {@Email}", user.Email);

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Request.Password);

        if (!isPasswordValid)
        {
            _logger.LogError("Invalid password for {@Email}", user.Email);
            return Result<LoginResponseModel>.Failure(Errors.InvalidCredentials);
        }

        var response = await CreateResponseAsync(user);

        return response;
    }

    private async Task<LoginResponseModel> CreateResponseAsync(ApplicationUser user)
    {
        _logger.LogInformation("Getting all {@Emaill} roles", user.Email);

        var roles = await _userManager.GetRolesAsync(user);

        _logger.LogInformation("Generating tokens for {@Emaill}", user.Email);

        var accessToken = _jwtProvider.GenerateToken(user, roles);

        var refreshToken = await _userManager.GenerateUserTokenAsync(
            user, RefreshToken.Provider, RefreshToken.Purpose);

        _logger.LogInformation("Saving refresh token for {@Email} to database", user.Email);

        await _userManager.SetAuthenticationTokenAsync(
            user, RefreshToken.Provider, RefreshToken.Name, refreshToken);

        var userDto = _mapper.Map<UserDto>(user);
        userDto.AccessToken = accessToken;
        userDto.Roles = roles;

        return new LoginResponseModel
        {
            User = userDto,
            RefreshToken = refreshToken,
        };
    }
}
