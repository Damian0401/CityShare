using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Enums;
using CityShare.Backend.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly CityShareDbContext _context;
    private readonly IClock _clock;
    private readonly ILogger<EventRepository> _logger;

    public EventRepository(
        CityShareDbContext context,
        IClock clock,
        ILogger<EventRepository> logger)
    {
        _context = context;
        _clock = clock;
        _logger = logger;
    }

    public async Task AddEventCategoriesAsync(IEnumerable<EventCategory> eventCategories, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding {@EventCategories} to database", eventCategories);
        _context.EventCategories.AddRange(eventCategories);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(Event eventToCreate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding {@Event} to database", eventToCreate);
        _context.Events.Add(eventToCreate);

        await _context.SaveChangesAsync(cancellationToken);

        return eventToCreate.Id;
    }

    public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking if event with id {@Id} exists in database", eventId);
        var eventExists = await _context.Events
            .AnyAsync(x => x.Id.Equals(eventId), cancellationToken);

        return eventExists;
    }

    public async Task<SearchEventDto?> GetByIdWithDetailsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for event with id {@Id} in database", eventId);
        var searchResult = await _context.Events
            .Include(x => x.Images)
            .Include(x => x.Address)
            .Include(x => x.EventCategories)
            .Where(x => x.Id.Equals(eventId))
            .Select(x => new SearchEventDto
            {
                Event = x,
                Likes = x.Likes.Count(),
                Comments = x.Comments.Count(),
                Author = x.Author.UserName ?? string.Empty
            }).AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return searchResult;
    }

    public async Task<(IEnumerable<SearchEventDto>, int)> GetByQueryAsync(EventSearchQueryDto eventQuery, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query from {@Query}", eventQuery);

        var query = _context.Events
            .Include(x => x.Images)
            .Include(x => x.Address)
            .Include(x => x.EventCategories)
            .AsQueryable();

        if (eventQuery.IsNow is not null && eventQuery.IsNow.Value)
        {
            _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.IsNow));
            query = query.Where(x => x.StartDate < _clock.Now && x.EndDate > _clock.Now);
        }

        if (eventQuery.StartDate is not null)
        {
            _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.StartDate));
            query = query.Where(x => x.EndDate > eventQuery.StartDate);
        }

        if (eventQuery.EndDate is not null)
        {
            _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.EndDate));
            query = query.Where(x => x.StartDate < eventQuery.EndDate);
        }

        if (eventQuery.Query is not null)
        {
            _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.Query));
            query = query.Where(x => x.Title.Contains(eventQuery.Query) ||
                x.Description.Contains(eventQuery.Query));
        }

        if (eventQuery.CityId is not null)
        {
            _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.CityId));
            query = query.Where(x => x.CityId.Equals(eventQuery.CityId));
        }

        if (eventQuery.SkipCategoryIds is not null)
        {
            var skipCategoryIds = eventQuery.SkipCategoryIds
                .Split(',')
                .Select(int.Parse)
                .ToList();

            _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.SkipCategoryIds));
            query = query.Where(x => !x.EventCategories.Select(c => c.CategoryId).Any(id => skipCategoryIds.Contains(id)));
        }

        _logger.LogInformation("Getting number of events for {@Name} from database", nameof(GetByQueryAsync));

        var count = await query.CountAsync(cancellationToken);

        if (eventQuery.SortBy is not null)
        {
            _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.SortBy));
            query = eventQuery.SortBy switch
            {
                EventSortByOptions.CreatedAtDesc => query.OrderByDescending(x => x.CreatedAt),
                EventSortByOptions.CreatedAtAsc => query.OrderBy(x => x.CreatedAt),
                EventSortByOptions.LikesDesc => query.OrderByDescending(x => x.Likes.Count()),
                EventSortByOptions.LikesAsc => query.OrderBy(x => x.Likes.Count()),
                _ => throw new InvalidStateException(),
            };
        }

        query = AddPagination(eventQuery, query);

        _logger.LogInformation("Executing {@Name} on database", nameof(GetByQueryAsync));
        var searchResult = await query.Select(x => new SearchEventDto
        {
            Event = x,
            Likes = x.Likes.Count(),
            Comments = x.Comments.Count(),
            Author = x.Author.UserName ?? string.Empty
        }).AsNoTracking().ToListAsync(cancellationToken);

        return (searchResult, count);
    }

    private IQueryable<Event> AddPagination(EventSearchQueryDto eventQuery, IQueryable<Event> query)
    {
        var pageSize = eventQuery.PageSize ?? Constants.MaxEventPageSize;

        if (eventQuery.PageNumber is not null)
        {
            _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.PageNumber));
            query = query.Skip(pageSize * eventQuery.PageNumber.Value - 1);
        }

        _logger.LogInformation("Adding {@Name} to query", nameof(eventQuery.PageSize));
        query.Take(pageSize);
        return query;
    }

    public async Task<int> GetImagesNumberAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting number of images for event with id {@Id} from database", eventId);
        var imagesNumber = await _context.Images
            .Where(x => x.EventId.Equals(eventId))
            .CountAsync();

        return imagesNumber;
    }

    public async Task<bool> IsLikedByUserAsync(Guid eventId, string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for like with user id {@UserId} and event id {@EventId} in database", userId, eventId);
        var likeExists = await _context.Likes
            .AnyAsync(x => x.EventId.Equals(eventId) && x.AuthorId.Equals(userId));

        return likeExists;
    }
}
