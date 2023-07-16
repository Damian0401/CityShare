using System.Globalization;

namespace CityShare.Backend.Api.Extensions;

public static class CommonExtension
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        var cultureInfo = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        return services;
    }
}
