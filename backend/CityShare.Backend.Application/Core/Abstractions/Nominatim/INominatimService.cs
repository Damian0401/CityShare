using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;

namespace CityShare.Backend.Application.Core.Abstractions.Nominatim;

public interface INominatimService
{
    Task<SearchDto?> SearchAsync(SearchParametersModel model, CancellationToken cancellationToken = default);
    Task<SearchDto?> SearchByQueryAsync(string query, CancellationToken cancellationToken = default);
}
