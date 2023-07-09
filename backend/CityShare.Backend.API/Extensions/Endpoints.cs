namespace CityShare.Backend.API.Extensions;

public static class Endpoints
{
    public static WebApplication AddEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        return app;
    }
}
