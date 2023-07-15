using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CityShare.Backend.Persistence;

public class CityShareDbContext : IdentityDbContext<ApplicationUser>
{
    public CityShareDbContext(DbContextOptions<CityShareDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserConfigurations).Assembly);

        base.OnModelCreating(builder);
    }
}
