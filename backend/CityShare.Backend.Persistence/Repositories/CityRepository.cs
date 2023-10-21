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
}
