using CityShare.Backend.Application.Core.Behaviors;
using CityShare.Backend.Application.Core.Behaviours;
using CityShare.Backend.Application.Core.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CityShare.Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddAutoMapper(assembly);

        services.AddValidatorsFromAssembly(assembly);

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipeloneBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddScoped<WebSocketsMiddleware>();

        return services;
    }
}
