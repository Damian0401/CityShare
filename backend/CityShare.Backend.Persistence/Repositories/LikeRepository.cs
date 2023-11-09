using CityShare.Backend.Application.Core.Abstractions.Likes;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly CityShareDbContext _context;
    private readonly ILogger<LikeRepository> _logger;

    public LikeRepository(CityShareDbContext context, ILogger<LikeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Like like, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding like {@Like} to database", like);
        _context.Likes.Add(like);

        await _context.SaveChangesAsync();
    }

    public async Task<int> GetGivenCountAsync(string userId, CancellationToken cancellationToken = default) 
    {
        _logger.LogInformation("Searching likes created by user with id {@Id}", userId);
        var count = await _context.Likes
            .Where(x => x.AuthorId.Equals(userId))
            .CountAsync();

        return count;
    }

    public async Task<int> GetReceivedCountAsync(string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching likes received by user with id {@Id}", userId);
        var count = await _context.Events
            .Where(x => x.AuthorId.Equals(userId))
            .Select(x => x.Likes.Count())
            .SumAsync();

        return count;
    }

    public async Task RemoveAsync(Guid eventId, string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Removing likes from event with id {@EventId} crete by user with id {UserId}", eventId, userId);
        await _context.Likes
            .Where(x => x.EventId.Equals(eventId) && x.AuthorId.Equals(userId))
            .ExecuteDeleteAsync(cancellationToken);
    }
}
