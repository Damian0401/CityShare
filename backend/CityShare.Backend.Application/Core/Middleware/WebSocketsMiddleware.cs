using CityShare.Backend.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace CityShare.Backend.Application.Core.Middleware;

public class WebSocketsMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;

        if (request.Path.StartsWithSegments("/hubs", StringComparison.OrdinalIgnoreCase) &&
            request.Query.TryGetValue(QueryParameters.AccessToken, out var accessToken))
        {
            request.Headers.Add(Headers.Authorization, $"Bearer {accessToken}");
        }

        await next(context);
    }
}