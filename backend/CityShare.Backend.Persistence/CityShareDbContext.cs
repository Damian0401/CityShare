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
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<EmailStatus> EmailStatuses { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<BoundingBox> BoundingBoxes { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<RequestStatus> RequestStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(CityShareDbContext).Assembly);

        base.OnModelCreating(builder);
    }
}
