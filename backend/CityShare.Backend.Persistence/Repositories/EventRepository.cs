using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly CityShareDbContext _context;
    private readonly ILogger<EventRepository> _logger;

    public EventRepository(
        CityShareDbContext context,
        ILogger<EventRepository> logger)
    {
        _context = context;
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
                CommentsNumber = x.Comments.Count()
            }).FirstOrDefaultAsync(cancellationToken);

        return searchResult;
    }

    public async Task<int> GetImagesNumberAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting number of images for event with id {@Id} from database", eventId);
        var imagesNumber = await _context.Images
            .Where(x => x.EventId.Equals(eventId))
            .CountAsync();

        return imagesNumber;
    }

    public async Task<bool> IsEventLikedAsync(Guid eventId, string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for like with user id {@UserId} and event id {@EventId} in database", userId, eventId);
        var likeExists = await _context.Likes
            .AnyAsync(x => x.EventId.Equals(eventId) && x.AuthorId.Equals(userId));

        return likeExists;
    }
}
