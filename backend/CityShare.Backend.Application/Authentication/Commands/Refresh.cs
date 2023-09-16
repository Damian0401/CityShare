using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Dtos.Authentication;
using CityShare.Backend.Application.Core.Dtos.Authentication.Refresh;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Authentication.Commands;

public class Refresh
{
    public record Command(RefreshRequestDto Request, string RefreshToken)
        : IRequest<Result<RefreshResponseDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Request.AccessToken)
                .NotEmpty()
                .WithName(x => nameof(x.Request.AccessToken));

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithName(x => nameof(x.RefreshToken));
        }
    }

    public class Handler : IRequestHandler<Command, Result<RefreshResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<Handler> _logger;

        public Handler(
            UserManager<ApplicationUser> userManager,
            IJwtProvider jwtProvider,
            IMapper mapper,
            ILogger<Handler> logger)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<RefreshResponseDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deserializing access token");
            var userEmail = _jwtProvider.GetEmailFromToken(request.Request.AccessToken);

            if (userEmail is null)
            {
                _logger.LogError("UserEmail not found in access token");
                return Result<RefreshResponseDto>.Failure(Errors.InvalidCredentials);
            }

            _logger.LogInformation("Searching for user with {@Email}", userEmail);
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user is null)
            {
                _logger.LogError("User with {@Email} not found", userEmail);
                return Result<RefreshResponseDto>.Failure(Errors.InvalidCredentials);
            }

            _logger.LogInformation("Checking if access token for {@Email} is valid", user.Email);
            var isTokenValid = await _userManager.VerifyUserTokenAsync(
                user, RefreshToken.Provider, RefreshToken.Purpose, request.RefreshToken);

            if (!isTokenValid)
            {
                _logger.LogError("Provided invalid refresh token for {@Email}", user.Email);
                return Result<RefreshResponseDto>.Failure(Errors.InvalidCredentials);
            }

            _logger.LogInformation("Getting all {@Emaill} roles", user.Email);
            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Generating new access token for {@Email}", user.Email);
            var accessToken = _jwtProvider.GenerateToken(user, roles);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.AccessToken = accessToken;
            userDto.Roles = roles;

            return new RefreshResponseDto(userDto);
        }
    }
}
