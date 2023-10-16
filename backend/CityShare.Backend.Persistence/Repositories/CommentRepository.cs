using CityShare.Backend.Application.Core.Abstractions.Comments;
using CityShare.Backend.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly CityShareDbContext _context;
    private readonly Logger<CommentRepository> _logger;

    public CommentRepository(
        CityShareDbContext context,
        Logger<CommentRepository> logger)
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
}
