using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Abstractions.Queue;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Infrastructure.Authentication;
using CityShare.Backend.Infrastructure.Cache;
using CityShare.Backend.Infrastructure.Emails;
using CityShare.Backend.Infrastructure.Nominatim;
using CityShare.Backend.Infrastructure.Queue;
using CityShare.Backend.Persistence;
using Microsoft.AspNetCore.Identity;
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
        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<IQueueService, StorageQueueService>();

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

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<RefreshTokenProvider<ApplicationUser>>(RefreshToken.Provider)
            .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>(EmailConfirmation.Provider)
            .AddEntityFrameworkStores<CityShareDbContext>();

        services.AddHttpClient<INominatimService, NominatimService>((serviveProvider, httpClient) =>
        {
            httpClient.DefaultRequestHeaders.UserAgent
                .Add(new(commonSettings.ApplicationName, commonSettings.ApplicationVersion));

            httpClient.BaseAddress = new(nominatimSettings.Url);
        });

        services.AddAzureClients(builder =>
        {
            builder.AddQueueServiceClient(configuration.GetConnectionString(ConnectionStrings.StorageAccount));
        });

        return services;
    }
}
