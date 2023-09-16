using CityShare.Backend.Application;
using CityShare.Backend.Application.Core.Middleware;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Infrastructure;
using CityShare.Backend.Persistence;
using CityShare.Backend.Persistence.Extensions;
using CityShare.Services.Api.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.SetUpCommon(configuration);
builder.Services.SetUpSecurity(configuration);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddPersistence(configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

var logger = app.Services.GetService<ILogger<Program>>();
ArgumentNullException.ThrowIfNull(logger);

logger.LogInformation("Seeding data...");
await app.Services.SeedDataAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseEndpoints();

app.UseCors(Cors.PolicyName);

app.UseAuthentication();
app.UseAuthorization();

logger.LogInformation("Starting app..");
app.Run();