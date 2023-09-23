using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Persistence;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Infrastructure.Events;

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
}
