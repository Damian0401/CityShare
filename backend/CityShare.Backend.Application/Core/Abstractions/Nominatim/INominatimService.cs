using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Models.Map.Reverse;
using CityShare.Backend.Application.Core.Models.Map.Search;
using CityShare.Backend.Application.Core.Models.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;

namespace CityShare.Backend.Application.Core.Abstractions.Nominatim;

public interface INominatimService
{
    Task<NominatimSearchResponseModel?> SearchAsync(
        NominatimSearchParametersModel model, 
        CancellationToken cancellationToken = default);
    Task<NominatimSearchResponseModel?> SearchByQueryAsync(
        string query, 
        CancellationToken cancellationToken = default);
    Task<NominatimReverseResponseModel?> ReverseAsync(
        double x,
        double y, 
        CancellationToken cancellationToken = default);
}
