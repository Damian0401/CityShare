using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CityShare.Backend.Api.Extensions;

public static class SecurityExtension
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = new CorsSettings();
        configuration.Bind(CorsSettings.Key, corsSettings);

        services.AddCors(options =>
        {
            options.AddPolicy(CorsSettings.PolicyName,
                policy => policy.WithMethods(corsSettings.ParsedMethods)
                    .WithOrigins(corsSettings.ParsedOrigins)
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.Key, jwtSettings);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
                ClockSkew = TimeSpan.Zero
            });

        services.AddAuthorization();

        services.Configure<RefreshTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromDays(jwtSettings.RefreshTokenExpirationDays);
        });

        return services;
    }
}
