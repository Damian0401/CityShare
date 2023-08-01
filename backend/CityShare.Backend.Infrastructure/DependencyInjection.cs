using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Infrastructure.Authentication;
using CityShare.Backend.Infrastructure.Cache;
using CityShare.Backend.Infrastructure.Nominatim;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthSettings>(configuration.GetSection(AuthSettings.Key));
        services.Configure<NominatimSettings>(configuration.GetSection(NominatimSettings.Key));
        services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.Key));
        services.Configure<CommonSettings>(configuration.GetSection(CommonSettings.Key));

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped(typeof(ICacheService<>), typeof(InMemoryCacheService<>));

        var commonSettings = new CommonSettings();
        configuration.Bind(CommonSettings.Key, commonSettings);

        services.AddHttpClient<INominatimService, NominatimService>((serviveProvider, httpClient) =>
        {
            var nominatimSettings = serviveProvider.GetRequiredService<IOptions<NominatimSettings>>().Value;

            httpClient.DefaultRequestHeaders.UserAgent
                .Add(new(commonSettings.ApplicationName, commonSettings.ApplicationVersion));

            httpClient.BaseAddress = new(nominatimSettings.Url);
        });

        return services;
    }
}
