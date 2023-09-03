using CityShare.Backend.Application.Core.Dtos.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Search;

namespace CityShare.Backend.Application.Core.Abstractions.Nominatim;

public interface INominatimService
{
    Task<NominatimSearchResponseDto?> SearchAsync(
        NominatimSearchRequestDto dto, 
        CancellationToken cancellationToken = default);
    Task<NominatimSearchResponseDto?> SearchByQueryAsync(
        string query, 
        CancellationToken cancellationToken = default);
    Task<NominatimReverseResponseDto?> ReverseAsync(
        double x,
        double y, 
        CancellationToken cancellationToken = default);
}
