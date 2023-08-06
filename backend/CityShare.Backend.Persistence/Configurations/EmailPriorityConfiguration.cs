using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class EmailPriorityConfiguration : IEntityTypeConfiguration<EmailPrirority>
{
    public void Configure(EntityTypeBuilder<EmailPrirority> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.RetryNumber) 
            .IsRequired();
    }
}
