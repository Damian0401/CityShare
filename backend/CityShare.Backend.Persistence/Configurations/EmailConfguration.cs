using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class EmailConfguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Body)
            .IsRequired()
            .HasMaxLength(3000);

        builder.Property(x => x.Receiver)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasOne(x => x.Status)
            .WithMany(x => x.Emails)
            .HasForeignKey(x => x.StatusId);
    }
}
