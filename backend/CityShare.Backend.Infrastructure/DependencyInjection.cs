using CityShare.Backend.Application.Core.Abstractions.Auth;
using CityShare.Backend.Application.Core.Abstractions.Blobs;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Maps;
using CityShare.Backend.Application.Core.Abstractions.Queues;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Infrastructure.Auth;
using CityShare.Backend.Infrastructure.Blobs;
using CityShare.Backend.Infrastructure.Cache;
using CityShare.Backend.Infrastructure.Emails;
using CityShare.Backend.Infrastructure.Maps;
using CityShare.Backend.Infrastructure.Queues;
using CityShare.Backend.Infrastructure.Utils;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityShare.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthSettings>(configuration.GetSection(AuthSettings.Key));
        services.Configure<NominatimSettings>(configuration.GetSection(NominatimSettings.Key));
        services.Configure<CacheSettings>(configuration.GetSection(CacheSettings.Key));
        services.Configure<CommonSettings>(configuration.GetSection(CommonSettings.Key));
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Key));

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ICacheService, InMemoryCacheService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IQueueService, StorageQueueService>();
        services.AddScoped<IBlobService, StorageBlobService>();

        services.AddSingleton<IClock, UtcClock>();

        var authSettings = new AuthSettings();
        configuration.Bind(AuthSettings.Key, authSettings);

        var nominatimSettings = new NominatimSettings();
        configuration.Bind(NominatimSettings.Key, nominatimSettings);

        var cacheSettings = new CacheSettings();
        configuration.Bind(CacheSettings.Key, cacheSettings);

        var commonSettings = new CommonSettings();
        configuration.Bind(CommonSettings.Key, commonSettings);

        services.AddMemoryCache(x => x.SizeLimit = cacheSettings.SizeLimit);

        services.Configure<RefreshTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromDays(authSettings.RefreshTokenExpirationDays);
        });

        services.Configure<EmailConfirmationTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromDays(authSettings.EmailConfirmationExpirationDays);
        });

        services.AddHttpClient<IMapService, NominatimService>((serviveProvider, httpClient) =>
        {
            httpClient.DefaultRequestHeaders.UserAgent
                .Add(new(commonSettings.ApplicationName, commonSettings.ApplicationVersion));

            httpClient.BaseAddress = new(nominatimSettings.Url);
        });

        services.AddAzureClients(builder =>
        {
            builder.AddQueueServiceClient(configuration.GetConnectionString(ConnectionStrings.StorageAccount));
            builder.AddBlobServiceClient(configuration.GetConnectionString(ConnectionStrings.StorageAccount));
        });

        return services;
    }
}
