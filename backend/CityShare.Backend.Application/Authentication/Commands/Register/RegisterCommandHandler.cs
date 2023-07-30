using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using CityShare.Backend.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Models.Authentication.Register;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponseModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager, 
        IJwtProvider jwtProvider,
        IMapper mapper,
        ILogger<RegisterCommandHandler> logger)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<RegisterResponseModel>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for user with {@Email}", request.Request.Email);
        var user = await _userManager.FindByEmailAsync(request.Request.Email);

        if (user is not null)
        {
            _logger.LogError("User with {@Email} not found", user.Email);
            return Result<RegisterResponseModel>.Failure(Errors.EmailTaken);
        }

        user = _mapper.Map<ApplicationUser>(request.Request);

        _logger.LogInformation("Creating new user for {@Email}", request.Request.Email);
        var result = await _userManager.CreateAsync(user, request.Request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => new Error(x.Code, x.Description))
                .ToList();

            _logger.LogError("Creating user failed with {@Errors}", errors);
            return Result<RegisterResponseModel>
                .Failure(errors);
        }

        _logger.LogInformation("Assigning {@Email} to {@User} role", user.Email, Roles.User);
        await _userManager.AddToRoleAsync(user, Roles.User);
        
        var response = await CreateResponseAsync(user);

        return response;
    }

    private async Task<RegisterResponseModel> CreateResponseAsync(ApplicationUser user)
    {
        _logger.LogInformation("Getting all {@Emaill} roles", user.Email);
        var roles = await _userManager.GetRolesAsync(user);

        _logger.LogInformation("Generating tokens for {@Email}", user.Email);
        var accessToken = _jwtProvider.GenerateToken(user, roles);
        var refreshToken = await _userManager.GenerateUserTokenAsync(
            user, RefreshToken.Provider, RefreshToken.Purpose);

        _logger.LogInformation("Saving refresh token for {@Email} to database", user.Email);
        await _userManager.SetAuthenticationTokenAsync(
            user, RefreshToken.Provider, RefreshToken.Name, refreshToken);

        var userDto = _mapper.Map<UserDto>(user);
        userDto.AccessToken = accessToken;
        userDto.Roles = roles;

        return new RegisterResponseModel
        {
            User = userDto,
            RefreshToken = refreshToken,
        };
    }
}
