using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CityShare.Services.Api.SetUps;

public static class Security
{
    public static IServiceCollection SetUpSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = new CorsSettings();
        configuration.Bind(CorsSettings.Key, corsSettings);

        services.AddCors(options =>
        {
            options.AddPolicy(Cors.PolicyName,
                policy => policy.WithMethods(corsSettings.ParsedMethods)
                    .WithOrigins(corsSettings.ParsedOrigins)
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        var authSettings = new AuthSettings();
        configuration.Bind(AuthSettings.Key, authSettings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authSettings.Issuer,
                ValidAudience = authSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecurityKey)),
                ClockSkew = TimeSpan.Zero
            });

        services.AddAuthorization();

        return services;
    }
}
