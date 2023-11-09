using CityShare.Backend.Application.Core.Abstractions.Comments;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly CityShareDbContext _context;
    private readonly ILogger<CommentRepository> _logger;

    public CommentRepository(
        CityShareDbContext context,
        ILogger<CommentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding comment to database");
        _context.Comments.Add(comment);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting comments from database for event with id {@Id}", eventId);
        var comments = await _context.Comments
            .Include(x => x.Author)
            .Where(x => x.EventId.Equals(eventId))
            .ToListAsync(cancellationToken);

        return comments;
    }

    public async Task<int> GetGivenCountAsync(string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for comments created by user with id {@Id}", userId);
        var count = await _context.Comments
            .Where(x => x.AuthorId.Equals(userId))
            .CountAsync();

        return count;
    }

    public async Task<int> GetReceivedCountAsync(string userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for comments received by user with id {@Id}", userId);
        var count = await _context.Events
            .Where(x => x.AuthorId.Equals(userId))
            .SelectMany(x => x.Comments)
            .Where(x => !x.AuthorId.Equals(userId))
            .CountAsync();

        return count;
    }
}
