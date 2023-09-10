using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace CityShare.Services.Api.Extensions;

public static class CommonExtension
{
    public static IServiceCollection SetUpCommon(this IServiceCollection services, IConfiguration configuration)
    {
        var cultureInfo = new CultureInfo(CultureInfos.EnUs);
        ValidatorOptions.Global.LanguageManager.Culture = cultureInfo;
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture(CultureInfos.EnUs);
        });

        services.AddEndpointsApiExplorer();

        var commonSettings = new CommonSettings();
        configuration.Bind(CommonSettings.Key, commonSettings);

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(commonSettings.ApplicationVersion, new OpenApiInfo 
            { 
                Title = commonSettings.ApplicationName, 
                Version = commonSettings.ApplicationName
            });
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[]{}
                }
            });
        });

        return services;
    }
}
