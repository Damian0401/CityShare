using CityShare.Backend.Api.Extensions;
using CityShare.Backend.Application;
using CityShare.Backend.Infrastructure;
using CityShare.Backend.Persistence;
using CityShare.Backend.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddCommon();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddPersistence(configuration);
builder.Services.AddSecurity(configuration);

var app = builder.Build();

await app.Services.SeedDataAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseEndpoints();

app.Run();
