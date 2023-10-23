using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Events.Queries;

public record GetEventsQuery(EventSearchQueryDto Request, string UserId) : IRequest<Result<PageWrapper<EventDto>>>;

public class GetEventsQueryValidator : AbstractValidator<GetEventsQuery>
{
    public GetEventsQueryValidator()
    {
        RuleFor(x => x.Request.PageNumber)
            .GreaterThan(0)
            .LessThanOrEqualTo(Constants.MaxEventPageSize)
            .WithName(x => nameof(x.Request.PageNumber));

        RuleFor(x => x.Request.EndDate)
            .LessThanOrEqualTo(x => x.Request.StartDate)
            .WithName(x => nameof(x.Request.EndDate));

        RuleFor(x => x.Request.SkipCategoryIds)
            .Matches("^\\d+(,\\d+)*$").WithMessage(x => $"{nameof(x.Request.SkipCategoryIds)} must be sequence of numbers separated by commas")
            .WithName(x => nameof(x.Request.SkipCategoryIds));
    }
}

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, Result<PageWrapper<EventDto>>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetEventsQueryHandler> _logger;

    public GetEventsQueryHandler(
        IEventRepository eventRepository,
        IMapper mapper,
        ILogger<GetEventsQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PageWrapper<EventDto>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting event and total count from {@Type}", _eventRepository.GetType());
        (var searchedEvents, var totalCount) = await _eventRepository.GetByQueryAsync(request.Request);

        _logger.LogInformation("Mapping {@Count} events to dtos", searchedEvents.Count());
        var dtos = searchedEvents.Select(e =>
        {
            var dto = _mapper.Map<EventDto>(e.Event);
            dto.Likes = e.Likes;
            dto.Comments = e.Comments;
            dto.Author = e.Author;

            return dto;
        }).ToList();

        _logger.LogInformation("Checking likes");
        foreach (var dto in dtos)
        {
            dto.IsLiked = await _eventRepository.IsLikedByUserAsync(dto.Id, request.UserId);
        }

        _logger.LogInformation("Creating response");
        var result = new PageWrapper<EventDto>
        {
            Content = dtos,
            TotalPages = GetTotalPagesNumber(request.Request.PageSize, totalCount),
            PageSize = request.Request.PageSize ?? Constants.MaxEventPageSize,
            PageNumber = request.Request.PageNumber ?? 1
        };

        return result;
    }

    private int GetTotalPagesNumber(int? pageSize, int totalCount)
    {
        _logger.LogInformation("Calculating total pages number for {0} = {1} and {2} = {3}", nameof(totalCount), totalCount, nameof(pageSize), pageSize);
        return (int)Math.Ceiling((double)totalCount / (pageSize ?? Constants.MaxEventPageSize));
    }
}