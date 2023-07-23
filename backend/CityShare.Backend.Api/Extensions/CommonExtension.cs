using CityShare.Backend.Domain.Constants;

namespace CityShare.Backend.Api.Extensions;

public static class CommonExtension
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { CultureInfos.EnUs };
            options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        return services;
    }
}
