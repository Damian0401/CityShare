using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class BoundingBoxConfiguration : IEntityTypeConfiguration<BoundingBox>
{
    public void Configure(EntityTypeBuilder<BoundingBox> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MaxX)
            .IsRequired();

        builder.Property(x => x.MaxY)
            .IsRequired();

        builder.Property(x => x.MinX)
            .IsRequired();

        builder.Property(x => x.MinY)
            .IsRequired();
    }
}
