using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Cities;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<City?> GeyByIdWithBoundingBoxAsync(int cityId, CancellationToken cancellationToken = default);
}
