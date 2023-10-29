using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityShare.Backend.Persistence.Data;

internal class Requests
{
    internal static async Task SeedRequestStatusesAsync(CityShareDbContext context)
    {
        var requestStatuses = typeof(RequestStatuses)
            .GetFields()
            .Select(x => x.GetValue(null))
            .Cast<string>();

        var existingRequestStatuses = context.RequestStatuses
            .AsNoTracking()
            .Select(x => x.Name)
            .ToList();

        foreach (var requestStatus in requestStatuses)
        {
            var statusExists = existingRequestStatuses.Contains(requestStatus);

            if (statusExists)
            {
                continue;
            }

            var newStatus = new RequestStatus
            {
                Name = requestStatus
            };

            context.RequestStatuses.Add(newStatus);
        }

        await context.SaveChangesAsync();
    }

    internal static async Task SeedRequestTypesAsync(CityShareDbContext context)
    {
        var requestTypes = typeof(RequestTypes)
            .GetFields()
            .Select(x => x.GetValue(null))
            .Cast<string>();

        var existingRequestTypes = context.RequestTypes
            .AsNoTracking()
            .Select(x => x.Name)
            .ToList();

        foreach (var requestType in requestTypes)
        {
            var statusExists = existingRequestTypes.Contains(requestType);

            if (statusExists)
            {
                continue;
            }

            var newStatus = new RequestType
            {
                Name = requestType
            };

            context.RequestTypes.Add(newStatus);
        }

        await context.SaveChangesAsync();
    }
}
