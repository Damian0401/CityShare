using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Auth;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Auth.Commands;

public record LoginCommand(LoginRequestDto Request)
    : IRequest<Result<AuthResponseDto>>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress()
            .WithName(x => nameof(x.Request.Email));

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .WithName(x => nameof(x.Request.Password));
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
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

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for user with {@Email}", request.Request.Email);
        var user = await _userManager.FindByEmailAsync(request.Request.Email);

        if (user is null)
        {
            _logger.LogError("User with {@Email} not found", request.Request.Email);
            return Result<AuthResponseDto>.Failure(Errors.InvalidCredentials);
        }

        _logger.LogInformation("Checking provided password for {@Email}", user.Email);
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Request.Password);

        if (!isPasswordValid)
        {
            _logger.LogError("Invalid password for {@Email}", user.Email);
            return Result<AuthResponseDto>.Failure(Errors.InvalidCredentials);
        }

        var response = await CreateResponseAsync(user);

        return response;
    }

    private async Task<AuthResponseDto> CreateResponseAsync(ApplicationUser user)
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

        return new AuthResponseDto(userDto, refreshToken);
    }
}
