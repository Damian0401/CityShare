using CityShare.Backend.Application.Core.Dtos.Comments;
using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Comments;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetCommentsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
}
