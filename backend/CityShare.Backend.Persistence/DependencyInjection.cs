﻿using CityShare.Backend.Application.Core.Abstractions.Categories;
using CityShare.Backend.Application.Core.Abstractions.Cities;
using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityShare.Backend.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.CityShareDB);

        services.AddDbContext<CityShareDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEventRepository, EventRepository>();

        return services;
    }
}
