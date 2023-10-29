using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityShare.Backend.Persistence.Data;

internal class Events
{
    internal static async Task SeedCategoriesAsync(CityShareDbContext context)
    {
        var categoriesAlreadyExists = context.Categories.Any();

        if (categoriesAlreadyExists)
        {
            return;
        }

        var categories = new List<Category>
        {
            new Category
            {
                Name = "Art"
            },
            new Category
            {
                Name = "Sport"
            },
            new Category
            {
                Name = "Food"
            },
            new Category
            {
                Name = "Charity"
            },
            new Category
            {
                Name = "Education"
            },
        };

        context.Categories.AddRange(categories);

        await context.SaveChangesAsync();
    }

    internal static async Task SeedCitiesAsync(CityShareDbContext context)
    {
        var citiesAlreadyExists = context.Cities.Any();

        if (citiesAlreadyExists)
        {
            return;
        }

        var cities = new List<City>
        {
            new City
            {
                Name = "Warszawa",
                BoundingBox = new BoundingBox
                {
                    MaxX = 52.3681531,
                    MinX = 52.0978497,
                    MaxY = 21.2711512,
                    MinY = 20.8516882,
                },
                Address = new Address
                {
                    DisplayName = "Warszawa, województwo mazowieckie, Polska",
                    X = 52.2337172,
                    Y = 21.0714322
                }
            },
            new City
            {
                Name = "Wrocław",
                BoundingBox = new BoundingBox
                {
                    MaxX = 51.2100604,
                    MinX = 51.0426686,
                    MaxY = 17.1762192,
                    MinY = 16.8073393,
                },
                Address = new Address
                {
                    DisplayName = "Wrocław, województwo dolnośląskie, Polska",
                    X = 51.1089776,
                    Y = 17.0326689
                }
            },
            new City
            {
                Name = "Kraków",
                BoundingBox = new BoundingBox
                {
                    MaxX = 50.1261338,
                    MinX = 49.9676668,
                    MaxY = 20.2173455,
                    MinY = 19.7922355,
                },
                Address = new Address
                {
                    DisplayName = "Kraków, województwo małopolskie, Polska",
                    X = 50.0619474,
                    Y = 19.9368564
                }
            },
        };

        context.Cities.AddRange(cities);

        await context.SaveChangesAsync();
    }
}
