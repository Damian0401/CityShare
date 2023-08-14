using CityShare.Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CityShare.Backend.Persistence;

public class CityShareDbContext : IdentityDbContext<ApplicationUser>
{
    public CityShareDbContext(DbContextOptions<CityShareDbContext> options)
        : base(options)
    {
    }

    public DbSet<Email> Emails { get; set; }
    public DbSet<EmailPrirority> EmailPriorities { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(CityShareDbContext).Assembly);

        base.OnModelCreating(builder);
    }
}
