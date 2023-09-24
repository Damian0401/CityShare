using CityShare.Backend.Application.Core.Abstractions.Categories;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CityShareDbContext _context;
    private readonly ILogger<CategoryRepository> _logger;

    public CategoryRepository(
        CityShareDbContext context,
        ILogger<CategoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<int>> GetAllIdsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all category ids from database");
        var categiryIds = await _context.Categories
            .Select(x => x.Id)
            .ToListAsync();

        return categiryIds;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all categories from database");
        var categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return categories;
    }
}
