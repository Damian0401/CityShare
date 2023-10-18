using CityShare.Backend.Application;
using CityShare.Backend.Application.Core.Hubs;
using CityShare.Backend.Application.Core.Middleware;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Infrastructure;
using CityShare.Backend.Persistence;
using CityShare.Backend.Persistence.Extensions;
using CityShare.Services.Api.Endpoints;
using CityShare.Services.Api.SetUps;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.SetUpCommon(configuration);
builder.Services.SetUpSecurity(configuration);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddPersistence(configuration);

builder.Services.AddSignalR();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

await app.Services.SeedDataAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<WebSocketsMiddleware>();

app.UseHttpsRedirection();

app.UseEndpoints();

app.UseCors(Cors.PolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<CommentHub>(Endpoints.Hubs.Comments);

app.Run();
