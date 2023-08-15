using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

public class EmailStatusConfiguration : IEntityTypeConfiguration<EmailStatus>
{
    public void Configure(EntityTypeBuilder<EmailStatus> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);
    }
}
