using CityShare.Backend.Application.Core.Abstractions.Maps;
using CityShare.Backend.Application.Core.Dtos.Maps;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Json;
using System.Text;

namespace CityShare.Backend.Infrastructure.Maps;

public class NominatimService : IMapService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NominatimService> _logger;

    public NominatimService(
        HttpClient httpClient, 
        ILogger<NominatimService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<MapReverseResponseDto?> ReverseAsync(
        double x, 
        double y, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var reverseQuery = $"reverse?format=json&zoom=18&addressdetails=0" +
            $"&lat={x.ToString(CultureInfo.InvariantCulture)}&lon={y.ToString(CultureInfo.InvariantCulture)}";

        _logger.LogInformation("Calling httpClient with {@Query}", reverseQuery);
        var result = await _httpClient.GetFromJsonAsync<MapReverseResponseDto>(
            reverseQuery, cancellationToken);

        if (result is null || result.error is not null)
        {
            _logger.LogWarning("Not found results for {@Query}", reverseQuery);
            return null;
        }

        return result;
    }

    public async Task<MapSearchResponseDto?> SearchAsync(
        MapSearchRequestDto dto, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var searchQuery = ParseParameters(dto);

        _logger.LogInformation("Calling httpClient with {@Query}", searchQuery);
        var result = await _httpClient.GetFromJsonAsync<MapSearchResponseDto[]>(
            searchQuery, cancellationToken);

        if (result is null || !result.Any())
        {
            return null;
        }

        _logger.LogInformation("Found {@Count} results, sorting", result.Count());
        var bestResult = result.OrderByDescending(x => x.importance).First();

        return bestResult;
    }

    public async Task<MapSearchResponseDto?> SearchByQueryAsync(
        string query, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var searchQuery = $"search?format=json&q={query}&addressdetails=0";

        _logger.LogInformation("Calling httpClient with {@Query}", searchQuery);
        var result = await _httpClient.GetFromJsonAsync<MapSearchResponseDto[]>(
            searchQuery, cancellationToken) ?? Array.Empty<MapSearchResponseDto>();

        if (!result.Any())
        {
            _logger.LogWarning("Not found results for {@Query}", searchQuery);
            return null;
        }

        _logger.LogInformation("Found {@Count} results, sorting", result.Count());
        var bestResult = result.OrderByDescending(x => x.importance).First();

        return bestResult;
    }

    private string ParseParameters(MapSearchRequestDto dto)
    {
        _logger.LogInformation("Parsing parameters {@Dto}", dto);

        var searchQuery = new StringBuilder("search?format=json&addressdetails=0");

        var fields = typeof(MapSearchRequestDto).GetProperties();

        foreach (var field in fields)
        {
            var value = field.GetValue(dto);

            if (value is null)
            {
                continue;
            }

            var name = field.Name.ToLower();

            searchQuery.Append($"&{name}={value}");
        }

        return searchQuery.ToString();
    }
}
