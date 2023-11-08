using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Likes;

public interface ILikeRepository
{
    Task AddAsync(Like like, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid eventId, string userId, CancellationToken cancellationToken = default);
    Task<int> GetReceivedCountAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> GetGivenCountAsync(string userId, CancellationToken cancellationToken = default);
}
