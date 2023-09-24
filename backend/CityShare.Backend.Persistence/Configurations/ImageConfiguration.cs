using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Uri)
            .HasMaxLength(256);

        builder.Property(x => x.ShouldBeBlurred)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.IsBlurred)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne<Event>()
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
