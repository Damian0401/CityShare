using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Events.Queries;

public record GetEventsByQuery(EventQueryDto Request) : IRequest<Result<PageWrapper<EventDto>>>;

public class GetEventsByQueryValidator : AbstractValidator<GetEventsByQuery>
{
    public GetEventsByQueryValidator()
    {
        RuleFor(x => x.Request.PageNumber)
            .GreaterThan(0)
            .LessThanOrEqualTo(Constants.DefaultEventPageSize)
            .WithName(x => nameof(x.Request.PageNumber));

        RuleFor(x => x.Request.EndDate)
            .LessThanOrEqualTo(x => x.Request.StartDate)
            .WithName(x => nameof(x.Request.EndDate));

        RuleFor(x => x.Request.SkipCategoryIds)
            .Matches("^\\d+(,\\d+)*$").WithMessage(x => $"{nameof(x.Request.SkipCategoryIds)} must be sequence of numbers separated by commas")
            .WithName(x => nameof(x.Request.SkipCategoryIds));
    }
}

public class GetEventsByQueryHandler : IRequestHandler<GetEventsByQuery, Result<PageWrapper<EventDto>>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetEventsByQueryHandler> _logger;

    public GetEventsByQueryHandler(
        IEventRepository eventRepository,
        IMapper mapper,
        ILogger<GetEventsByQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PageWrapper<EventDto>>> Handle(GetEventsByQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting event and total count from {@Type}", _eventRepository.GetType());
        (var searchedEvents, var totalCount) = await _eventRepository.GetByQueryAsync(request.Request);

        _logger.LogInformation("Mapping {@Count} events to dtos", searchedEvents.Count());
        var dtos = searchedEvents.Select(e =>
        {
            var dto = _mapper.Map<EventDto>(e.Event);
            dto.Likes = e.Likes;
            dto.CommentNumber = e.CommentNumber;

            return dto;
        });

        _logger.LogInformation("Creating response");
        var result = new PageWrapper<EventDto>
        {
            Content = dtos,
            TotalPages = GetTotalPagesNumber(request.Request.PageSize, totalCount),
            PageSize = request.Request.PageSize ?? Constants.DefaultEventPageSize,
            PageNumber = request.Request.PageNumber ?? 1
        };

        return result;
    }

    private int GetTotalPagesNumber(int? pageSize, int totalCount)
    {
        _logger.LogInformation("Calculating total pages number for {0} = {1} and {2} = {3}", nameof(totalCount), totalCount, nameof(pageSize), pageSize);
        return (int)Math.Ceiling((double)totalCount / (pageSize ?? Constants.DefaultEventPageSize));
    }
}