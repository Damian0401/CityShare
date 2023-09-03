using CityShare.Backend.Application.Core.Dtos.Authentication.Register;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ConfirmEmailCommandHandler> _logger;

    public ConfirmEmailCommandHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<ConfirmEmailCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        (var id, var token) = request.Request;

        _logger.LogInformation("Searching for user with id: {@Id}", id);
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            _logger.LogError("User with id {@Id} not found", id);
            return Result.Failure(Errors.InvalidToken);
        }

        if (user.EmailConfirmed)
        {
            _logger.LogError("Email for user {@User} already confirmed", user);
            return Result.Failure(Errors.EmailAlreadyConfirmed);
        }

        _logger.LogInformation("Confirming email with token", token);
        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(x => new Error(x.Code, x.Description))
                .ToList();

            _logger.LogError("Email confirmation failed with {@Errors}", errors);
            return Result<RegisterResponseDto>
                .Failure(errors);
        }

        return Result.Success();
    }
}
