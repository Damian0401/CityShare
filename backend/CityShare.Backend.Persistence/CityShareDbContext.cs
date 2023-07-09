using Microsoft.EntityFrameworkCore;

namespace CityShare.Backend.Persistence;

public class CityShareDbContext : DbContext
{
    public CityShareDbContext(DbContextOptions<CityShareDbContext> options)
        : base(options)
    {
        
    }
}
