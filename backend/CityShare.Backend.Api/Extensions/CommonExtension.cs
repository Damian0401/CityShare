using CityShare.Backend.Domain.Constants;
using FluentValidation;
using System.Globalization;

namespace CityShare.Backend.Api.Extensions;

public static class CommonExtension
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(CultureInfos.EnUs);

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture(CultureInfos.EnUs);
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        return services;
    }
}
