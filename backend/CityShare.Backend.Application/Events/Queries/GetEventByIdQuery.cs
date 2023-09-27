using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Extensions;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Events.Queries;

public record GetEventByIdQuery(Guid EventId, string UserId) : IRequest<Result<EventDto>>;

public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdQueryValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Result<EventDto>>
{
    private readonly IEventRepository _eventRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<GetEventByIdQueryHandler> _logger;

    public GetEventByIdQueryHandler(
        IEventRepository eventRepository,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        ILogger<GetEventByIdQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<EventDto>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for event with id {@Id}", request.EventId);
        var searchResult = await _eventRepository.GetByIdWithDetailsAsync(request.EventId);

        if (searchResult is null)
        {
            _logger.LogError("Event with id {@Id} now found", request.EventId);
            return Result<EventDto>.Failure(Errors.NotFound);
        }

        _logger.LogInformation("Mapping event with id {@Id} to dto", request.EventId);
        var eventDto = _mapper.Map<EventDto>(searchResult.Event);
        eventDto.Likes = searchResult.Likes;
        eventDto.CommentNumber = searchResult.CommentsNumber;

        _logger.LogInformation("Searching for UserName of user with id {@Id}", searchResult.Event.AuthorId);
        eventDto.Author = await _userManager.GetUserNameByIdAsync(searchResult.Event.AuthorId);

        _logger.LogInformation("Checking if user with id {@UserId} likes event with id {@EventId}", request.UserId, request.EventId);
        eventDto.IsLiked = await _eventRepository.IsEventLikedAsync(searchResult.Event.Id, request.UserId, cancellationToken);

        return eventDto;
    }
}