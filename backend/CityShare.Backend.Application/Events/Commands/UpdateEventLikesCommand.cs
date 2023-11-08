using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Likes;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Events.Commands;

public record UpdateEventLikesCommand(Guid EventId, string AuthorId) : IRequest<Result>;

public class UpdateEventLikesCommandValidator : AbstractValidator<UpdateEventLikesCommand>
{
    public UpdateEventLikesCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.AuthorId) 
            .NotEmpty();
    }
}

public class UpdateEventLikesCommandHandler : IRequestHandler<UpdateEventLikesCommand, Result>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateEventLikesCommandHandler> _logger;

    public UpdateEventLikesCommandHandler(
        IEventRepository eventRepository,
        ILikeRepository likeRepository,
        IMapper mapper,
        ILogger<UpdateEventLikesCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _likeRepository = likeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateEventLikesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking if event with id {@EventId} is liked by user with id {@AuthorId}", request.EventId, request.AuthorId);
        var isLiked = await _eventRepository.IsLikedByUserAsync(request.EventId, request.AuthorId, cancellationToken);

        if (isLiked)
        {
            _logger.LogInformation("Removing like form event with id {@EventId} from user with id {@AuthorId}", request.EventId, request.AuthorId);
            await _likeRepository.RemoveAsync(request.EventId, request.AuthorId, cancellationToken);

            return Result.Success();
        }

        _logger.LogInformation("Adding like to event with id {@EventId} from user with id {@AuthorId}", request.EventId, request.AuthorId);
        var like = _mapper.Map<Like>(request);
        await _likeRepository.AddAsync(like, cancellationToken);

        return Result.Success();
    }
}
