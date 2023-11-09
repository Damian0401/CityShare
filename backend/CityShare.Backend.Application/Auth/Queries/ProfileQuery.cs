using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Likes;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Auth.Queries;

public record ProfileQuery(string UserId) : IRequest<Result<ProfileDto>>;

public class ProfileQueryValidator : AbstractValidator<ProfileQuery>
{
    public ProfileQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithName(x => nameof(x.UserId));
    }
}

public class ProfileQueryHandler : IRequestHandler<ProfileQuery, Result<ProfileDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEventRepository _eventRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly ILogger<ProfileQueryHandler> _logger;

    public ProfileQueryHandler(UserManager<ApplicationUser> userManager,
        IEventRepository eventRepository,
        ILikeRepository likeRepository,
        ILogger<ProfileQueryHandler> logger)
    {
        _userManager = userManager;
        _eventRepository = eventRepository;
        _likeRepository = likeRepository;
        _logger = logger;
    }

    public async Task<Result<ProfileDto>> Handle(ProfileQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for user with id {@Id}", request.UserId);
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            _logger.LogInformation("User with id {@Id} not found", request.UserId);
            return Result<ProfileDto>.Failure(Errors.NotFound);
        }

        _logger.LogInformation("Getting number of event create by user with id {@Id}", request.UserId);
        var createdEvents = await _eventRepository.GetCreatedCountAsync(request.UserId, cancellationToken);

        _logger.LogInformation("Getting number of likes given by user with id {@Id}", request.UserId);
        var givenLikes = await _likeRepository.GetGivenCountAsync(request.UserId, cancellationToken);

        _logger.LogInformation("Getting number of likes received by user with id {@Id}", request.UserId);
        var receivedLikes = await _likeRepository.GetReceivedCountAsync(request.UserId, cancellationToken);

        var response = new ProfileDto
        {
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            CreatedEvents = createdEvents,
            GivenLikes = givenLikes,
            ReceivedLikes = receivedLikes,
        };

        return response;
    }
}