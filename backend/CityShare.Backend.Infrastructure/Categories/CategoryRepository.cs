using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Categories;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Infrastructure.Categories;

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

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        _logger.LogInformation("Getting all categories from database");
        var categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync();

        return categories;
    }
}
