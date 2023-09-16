using CityShare.Backend.Application.Core.Dtos.Authentication.ConfirmEmail;
using CityShare.Backend.Application.Core.Dtos.Authentication.Register;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Authentication.Commands;

public class ConfirmEmail
{
    public record Command(EmailConfirmRequestDto Request) 
        : IRequest<Result>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Request.Id)
                .NotEmpty()
                .WithName(x => nameof(x.Request.Id));

            RuleFor(x => x.Request.Token)
                .NotEmpty()
                .WithName(x => x.Request.Token);
        }
    }

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<Handler> _logger;

        public Handler(
            UserManager<ApplicationUser> userManager,
            ILogger<Handler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
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
}
