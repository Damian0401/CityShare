using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Author)
            .WithMany(x => x.Requests)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Event)
            .WithMany(x => x.Requests)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Status)
            .WithMany(x => x.Requests)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Type)
            .WithMany(x => x.Requests)
            .HasForeignKey(x => x.TypeId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
