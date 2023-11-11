using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Comments;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetCommentsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<int> GetReceivedCountAsync(string userId, CancellationToken cancellationToken = default);
    Task<int> GetGivenCountAsync(string userId, CancellationToken cancellationToken = default);
}
