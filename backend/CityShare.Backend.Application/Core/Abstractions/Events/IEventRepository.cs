using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Events;

public interface IEventRepository
{
    Task<Guid> CreateAsync(Event entity, CancellationToken cancellationToken = default);
    Task AddEventCategoriesAsync(IEnumerable<EventCategory> eventCategories, CancellationToken cancellationToken = default);
}
