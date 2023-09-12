using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne<Event>()
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne<ApplicationUser>()
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
