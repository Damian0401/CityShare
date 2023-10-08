using CityShare.Backend.Application.Core.Dtos.Maps;

namespace CityShare.Backend.Application.Core.Abstractions.Maps;

public interface IMapService
{
    Task<MapSearchResponseDto?> SearchAsync(
        MapSearchRequestDto dto, 
        CancellationToken cancellationToken = default);
    Task<MapSearchResponseDto?> SearchByQueryAsync(
        string query, 
        CancellationToken cancellationToken = default);
    Task<MapReverseResponseDto?> ReverseAsync(
        double x,
        double y, 
        CancellationToken cancellationToken = default);
}
