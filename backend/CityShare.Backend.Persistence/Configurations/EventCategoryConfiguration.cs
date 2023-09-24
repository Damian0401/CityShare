using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
{
    public void Configure(EntityTypeBuilder<EventCategory> builder)
    {
        builder.HasKey(x => new { x.EventId, x.CategoryId });

        builder.HasOne<Event>()
            .WithMany(x => x.EventCategories)
            .HasForeignKey(x => x.EventId);

        builder.HasOne<Category>()
            .WithMany(x => x.EventCategories)
            .HasForeignKey(x => x.CategoryId);
    }
}
