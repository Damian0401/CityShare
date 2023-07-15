using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityShare.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Key));

        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }
}
