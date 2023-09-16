﻿using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Queue;
using CityShare.Backend.Application.Core.Dtos.Authentication;
using CityShare.Backend.Application.Core.Dtos.Authentication.Register;
using CityShare.Backend.Application.Core.Dtos.Emails.Create;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace CityShare.Backend.Application.Authentication.Commands;

public class Register
{
    public record Command(RegisterRequestDto Request)
        : IRequest<Result<RegisterResponseDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256)
                .WithName(x => nameof(x.Request.Email));

            RuleFor(x => x.Request.UserName)
                .NotEmpty()
                .MaximumLength(256)
                .MinimumLength(6)
                .Matches("^[a-zA-Z0-9]+$").WithMessage(x => $"'{nameof(x.Request.UserName)}' should contain only letters and digits.")
                .WithName(x => nameof(x.Request.UserName));

            RuleFor(x => x.Request.Password)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("[A-Z]").WithMessage(x => $"'{nameof(x.Request.Password)}' must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage(x => $"'{nameof(x.Request.Password)}' must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage(x => $"'{nameof(x.Request.Password)}' must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage(x => $"'{nameof(x.Request.Password)}' must contain at least one non-alphanumeric character.")
                .WithName(x => nameof(x.Request.Password));

        }
    }

    public class Handler : IRequestHandler<Command, Result<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;
        private readonly IQueueService _queueService;
        private readonly IEmailRepository _emailRepository;
        private readonly CommonSettings _commonSettings;
        private readonly ILogger<Handler> _logger;

        public Handler(
            UserManager<ApplicationUser> userManager,
            IJwtProvider jwtProvider,
            IMapper mapper,
            IOptions<CommonSettings> options,
            IQueueService queueService,
            IEmailRepository emailRepository,
            ILogger<Handler> logger)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _queueService = queueService;
            _emailRepository = emailRepository;
            _commonSettings = options.Value;
            _logger = logger;
        }

        public async Task<Result<RegisterResponseDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for user with {@Email}", request.Request.Email);
            var user = await _userManager.FindByEmailAsync(request.Request.Email);

            if (user is not null)
            {
                _logger.LogError("User with {@Email} already exists", user.Email);
                return Result<RegisterResponseDto>.Failure(Errors.EmailTaken);
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
                return Result<RegisterResponseDto>
                    .Failure(errors);
            }

            _logger.LogInformation("Assigning {@Email} to {@User} role", user.Email, Roles.User);
            await _userManager.AddToRoleAsync(user, Roles.User);

            _logger.LogInformation("Sending welcome email to {@Email}", user.Email);
            await SendWelcomeEmail(request, user, cancellationToken);

            _logger.LogInformation("Creating response");
            var response = await CreateResponseAsync(user);

            return response;
        }

        private async Task SendWelcomeEmail(Command request, ApplicationUser user, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Generating email confirmation token");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);

            _logger.LogInformation("Creating CreateEmailDto");
            var dto = new CreateEmailDto(
                request.Request.Email,
                EmailTemplates.WelcomeAndEmailConfirmLink,
                EmailPriorities.Medium,
                new Dictionary<string, string>
                {
                {EmailPlaceholders.Id, user.Id },
                {EmailPlaceholders.Token, encodedToken },
                {EmailPlaceholders.UserName, request.Request.UserName },
                {EmailPlaceholders.ClientUrl, _commonSettings.ClientUrl },
                });

            _logger.LogInformation("Creating email from dto {@Dto}", dto);
            var emailId = await _emailRepository.CreateAsync(dto, cancellationToken);

            _logger.LogInformation("Sending emailId {@Id} to queue {@Queue}", emailId, QueueNames.EmailsToSend);
            var options = new QueueServiceOptions
            {
                CreateIfNotExists = true,
                EncodeToBase64 = true,
            };
            await _queueService.SendAsync(QueueNames.EmailsToSend, emailId, options, cancellationToken);
        }

        private async Task<RegisterResponseDto> CreateResponseAsync(ApplicationUser user)
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

            _logger.LogInformation("Mapping user {@User} to dto", user);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.AccessToken = accessToken;
            userDto.Roles = roles;

            return new RegisterResponseDto(userDto, refreshToken);
        }
    }
}
