using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Infrastructure.Auth;
using CityShare.Backend.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CityShare.Services.Api.SetUps;

public static class Security
{
    public static IServiceCollection SetUpSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<RefreshTokenProvider<ApplicationUser>>(RefreshToken.Provider)
            .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>(EmailConfirmation.Provider)
            .AddEntityFrameworkStores<CityShareDbContext>();

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

        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.RequireAdminRole, policy => policy.RequireRole(Roles.Admin))
            .AddPolicy(Policies.RequireUserRole, policy => policy.RequireRole(Roles.User));

        return services;
    }
}
