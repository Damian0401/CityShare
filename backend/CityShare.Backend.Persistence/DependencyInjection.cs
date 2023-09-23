using CityShare.Backend.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityShare.Backend.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.CityShareDB);

        services.AddDbContext<CityShareDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

        return services;
    }
}
