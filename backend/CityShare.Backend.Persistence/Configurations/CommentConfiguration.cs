using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne<Event>()
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
