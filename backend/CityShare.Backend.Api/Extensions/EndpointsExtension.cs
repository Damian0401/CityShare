using CityShare.Backend.Api.Api;

namespace CityShare.Backend.Api.Extensions;

public static class EndpointsExtension
{
    public static WebApplication UseEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        app.MapPost(Endpoints.V1.Auth.Register, Api.V1.Authentication.Register)
            .AllowAnonymous();

        app.MapPost(Endpoints.V1.Auth.Login, Api.V1.Authentication.Login)
            .AllowAnonymous();

        app.MapPost(Endpoints.V1.Auth.Refresh, Api.V1.Authentication.Refresh)
            .AllowAnonymous();

        app.MapGet(Endpoints.V1.Map.Search, Api.V1.Map.Search)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Map.Reverse, Api.V1.Map.Reverse)
            .RequireAuthorization();

        return app;
    }
}
