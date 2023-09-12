using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<City>(x => x.AddressId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.BoundingBox)
            .WithOne()
            .HasForeignKey<City>(x => x.BoundingBoxId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
