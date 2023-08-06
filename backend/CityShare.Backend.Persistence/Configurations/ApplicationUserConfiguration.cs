using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityShare.Backend.Persistence.Configurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Do NOT modify configuration of build-in fields
        // This may lead to strange behaviours
    }
}
