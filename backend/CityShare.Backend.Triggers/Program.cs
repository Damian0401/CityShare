using CityShare.Backend.Application;
using CityShare.Backend.Infrastructure;
using CityShare.Backend.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("serilog.json", optional: false, reloadOnChange: false)
            .AddJsonFile("local.serilog.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();
    })
    .UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    })
    .ConfigureServices((host, services) =>
    {
        services.AddApplication();
        services.AddInfrastructure(host.Configuration);
        services.AddPersistence(host.Configuration);
    })
    .Build();

host.Run();
