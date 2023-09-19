using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(3000);

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Author)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.City)
            .WithOne()
            .HasForeignKey<Event>(x => x.CityId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<Event>(x => x.AddressId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
