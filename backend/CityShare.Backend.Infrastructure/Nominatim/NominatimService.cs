using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Search;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Json;
using System.Text;

namespace CityShare.Backend.Infrastructure.Nominatim;

public class NominatimService : INominatimService
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

    public async Task<NominatimReverseResponseDto?> ReverseAsync(
        double x, 
        double y, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var reverseQuery = $"reverse?format=json&zoom=18&addressdetails=0" +
            $"&lat={x.ToString(CultureInfo.InvariantCulture)}&lon={y.ToString(CultureInfo.InvariantCulture)}";

        _logger.LogInformation("Calling httpClient with {@Query}", reverseQuery);
        var result = await _httpClient.GetFromJsonAsync<NominatimReverseResponseDto>(
            reverseQuery, cancellationToken);

        if (result is null)
        {
            _logger.LogWarning("Not found results for {@Query}", reverseQuery);
            return null;
        }

        return result;
    }

    public async Task<NominatimSearchResponseDto?> SearchAsync(
        NominatimSearchRequestDto dto, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var searchQuery = ParseParameters(dto);

        _logger.LogInformation("Calling httpClient with {@Query}", searchQuery);
        var result = await _httpClient.GetFromJsonAsync<NominatimSearchResponseDto[]>(
            searchQuery, cancellationToken);

        if (result is null || !result.Any())
        {
            return null;
        }

        _logger.LogInformation("Found {@Count} results, sorting", result.Count());
        var bestResult = result.OrderByDescending(x => x.importance).First();

        return bestResult;
    }

    public async Task<NominatimSearchResponseDto?> SearchByQueryAsync(
        string query, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var searchQuery = $"search?format=json&q={query}&addressdetails=0";

        _logger.LogInformation("Calling httpClient with {@Query}", searchQuery);
        var result = await _httpClient.GetFromJsonAsync<NominatimSearchResponseDto[]>(
            searchQuery, cancellationToken) ?? Array.Empty<NominatimSearchResponseDto>();

        if (!result.Any())
        {
            _logger.LogWarning("Not found results for {@Query}", searchQuery);
            return null;
        }

        _logger.LogInformation("Found {@Count} results, sorting", result.Count());
        var bestResult = result.OrderByDescending(x => x.importance).First();

        return bestResult;
    }

    private string ParseParameters(NominatimSearchRequestDto dto)
    {
        _logger.LogInformation("Parsing parameters {@Dto}", dto);

        var searchQuery = new StringBuilder("search?format=json&addressdetails=0");

        var fields = typeof(NominatimSearchRequestDto).GetProperties();

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
