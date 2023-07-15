using CityShare.Backend.Api.Api;
using CityShare.Backend.Api.Api.V1;

namespace CityShare.Backend.Api.Extensions;

public static class EndpointsExtension
{
    public static WebApplication UseEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        app.MapPost(Endpoints.V1.Register, Authorization.Register)
            .AllowAnonymous();

        return app;
    }
}
