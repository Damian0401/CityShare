using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class RequestRepository : IRequestRepository
{
    private readonly CityShareDbContext _context;
    private readonly ILogger<RequestRepository> _logger;

    public RequestRepository(
        CityShareDbContext context,
        ILogger<RequestRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CreateAsync(Request request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding {@Request} to database", request);
        _context.Requests.Add(request);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetStatusIdAsync(string statusName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for status with name {@Name} in database", statusName);
        var status = await _context.RequestStatuses
            .AsNoTracking()
            .FirstAsync(x => x.Name.Equals(statusName), cancellationToken);

        return status.Id;
    }

    public async Task<IEnumerable<RequestType>> GetTypesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all types from database");
        var types = await _context.RequestTypes
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return types;
    }

    public async Task<bool> TypeExistsAsync(int typeId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching for type with id {@Id} in database", typeId);
        var exsits = await _context.RequestTypes
            .AnyAsync(x => x.Id.Equals(typeId), cancellationToken);

        return exsits;
    }
}
