using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Contracts.Authentication.Refresh;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Authentication.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<RefreshResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<RefreshCommandHandler> _logger;

    public RefreshCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtProvider jwtProvider,
        IMapper mapper,
        ILogger<RefreshCommandHandler> logger)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<RefreshResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deserializing access token");

        var userEmail = _jwtProvider.GetEmailFromToken(request.Request.AccessToken);

        if (userEmail is null)
        {
            return Result<RefreshResponse>.Failure(Errors.InvalidCredentials);
        }

        _logger.LogInformation("Searching for user with {@Email}", userEmail);

        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is null)
        {
            return Result<RefreshResponse>.Failure(Errors.InvalidCredentials);
        }

        _logger.LogInformation("Checking if access token for {@Email} is valid", user.Email);

        var isTokenValid = await _userManager.VerifyUserTokenAsync(
            user, RefreshToken.Provider, RefreshToken.Purpose, request.RefreshToken);

        if (!isTokenValid)
        {
            return Result<RefreshResponse>.Failure(Errors.InvalidCredentials);
        }

        _logger.LogInformation("Generating new access token for {@Email}", user.Email);

        var accessToken = _jwtProvider.GenerateToken(user);

        var userDto = _mapper.Map<UserDto>(user);
        userDto.AccessToken = accessToken;

        return new RefreshResponse(userDto);
    }
}
