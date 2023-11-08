using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Likes;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
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
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetEventByIdQueryHandler> _logger;

    public GetEventByIdQueryHandler(
        IEventRepository eventRepository,
        ILikeRepository likeRepository,
        IMapper mapper,
        ILogger<GetEventByIdQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _likeRepository = likeRepository;
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
        eventDto.Comments = searchResult.Comments;
        eventDto.Author = searchResult.Author;

        _logger.LogInformation("Checking if user with id {@UserId} likes event with id {@EventId}", request.UserId, request.EventId);
        eventDto.IsLiked = await _eventRepository.IsLikedByUserAsync(searchResult.Event.Id, request.UserId, cancellationToken);

        return eventDto;
    }
}