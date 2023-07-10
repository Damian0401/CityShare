using CityShare.Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CityShare.Backend.Persistence;

public class CityShareDbContext : IdentityDbContext<ApplicationUser>
{
    public CityShareDbContext(DbContextOptions<CityShareDbContext> options)
        : base(options)
    {
        
    }
}
