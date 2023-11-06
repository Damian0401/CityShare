using CityShare.Backend.Application.Core.Abstractions.Cities;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class CityRepository : ICityRepository
{
    private readonly CityShareDbContext _context;
    private readonly ILogger<CityRepository> _logger;

    public CityRepository(
        CityShareDbContext context, 
        ILogger<CityRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<City?> GeyByIdWithBoundingBoxAsync(int cityId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting city with id {@Id} from database", cityId);
        var city = await _context.Cities
            .Include(x => x.BoundingBox)
            .FirstOrDefaultAsync(x => x.Id.Equals(cityId), cancellationToken);

        return city;
    }

    public async Task<IEnumerable<City>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all cities from database");
        var cities = await _context.Cities
            .Include(x => x.Address)
            .Include(x => x.BoundingBox)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return cities;
    }

    public async Task<bool> ExistsAsync(int cityId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking if city with id {@Id} exists in database", cityId);
        var exists = await _context.Cities.AnyAsync(x => x.Id.Equals(cityId), cancellationToken);

        return exists;
    }
}
