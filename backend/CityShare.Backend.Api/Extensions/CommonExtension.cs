using CityShare.Backend.Domain.Constants;
using FluentValidation;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace CityShare.Backend.Api.Extensions;

public static class CommonExtension
{
    public static IServiceCollection SetUpCommon(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(CultureInfos.EnUs);

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture(CultureInfos.EnUs);
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "CityShare API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        return services;
    }
}
