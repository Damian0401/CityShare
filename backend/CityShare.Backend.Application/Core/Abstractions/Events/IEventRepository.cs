using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Events;

public interface IEventRepository
{
    Task<(IEnumerable<SearchEventDto>, int)> GetByQueryAsync(EventSearchQueryDto eventQuery, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Event eventToCreate, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<SearchEventDto?> GetByIdWithDetailsAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<int> GetImagesNumberAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task AddEventCategoriesAsync(IEnumerable<EventCategory> eventCategories, CancellationToken cancellationToken = default);
    Task<bool> IsLikedByUserAsync(Guid eventId, string userId, CancellationToken cancellationToken = default);
    Task<int> GetCreatedCountAsync(string userId, CancellationToken cancellationToken = default);
}
