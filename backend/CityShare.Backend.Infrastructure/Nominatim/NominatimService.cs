using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Models.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Json;
using System.Text;

namespace CityShare.Backend.Infrastructure.Nominatim;

public class NominatimService : INominatimService
{
    private readonly HttpClient _httpClient;
    private readonly ICacheService _cacheService;
    private readonly ILogger<NominatimService> _logger;

    public NominatimService(
        HttpClient httpClient, 
        ICacheService cacheService,
        ILogger<NominatimService> logger)
    {
        _httpClient = httpClient;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<NominatimReverseResponseModel?> ReverseAsync(double x, double y, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var reverseQuery = $"reverse?format=json&zoom=18&addressdetails=0" +
            $"&lat={x.ToString(CultureInfo.InvariantCulture)}&lon={y.ToString(CultureInfo.InvariantCulture)}";

        _logger.LogInformation("Checking for {@Query} in cacheService", reverseQuery);
        if (_cacheService.TryGet<NominatimReverseResponseModel>(reverseQuery, out var cachedDto))
        {
            _logger.LogInformation("Returning cached dto {@Dto}", cachedDto);
            return cachedDto;
        }

        _logger.LogInformation("Calling httpClient with {@Query}", reverseQuery);
        var result = await _httpClient.GetFromJsonAsync<NominatimReverseResponseModel>(
            reverseQuery, cancellationToken);

        if (result is null)
        {
            _logger.LogWarning("Not found results for {@Query}", reverseQuery);
            return null;
        }

        _logger.LogInformation("Caching {@Result}", result);
        _cacheService.Set(reverseQuery, result);

        return result;
    }

    public async Task<NominatimSearchResponseModel?> SearchAsync(NominatimSearchParametersModel model, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var searchQuery = ParseParameters(model);

        _logger.LogInformation("Checking for {@Query} in cacheService", searchQuery);
        if (_cacheService.TryGet<NominatimSearchResponseModel>(searchQuery, out var cachedDto))
        {
            _logger.LogInformation("Returning cached dto {@Dto}", cachedDto);
            return cachedDto;
        }

        _logger.LogInformation("Calling httpClient with {@Query}", searchQuery);
        var result = await _httpClient.GetFromJsonAsync<NominatimSearchResponseModel[]>(
            searchQuery, cancellationToken);

        if (result is null || !result.Any())
        {
            return null;
        }

        _logger.LogInformation("Found {@Count} results, sorting", result.Count());
        var bestResult = result.OrderByDescending(x => x.importance).First();

        _logger.LogInformation("Caching best result {@Result}", bestResult);
        _cacheService.Set(searchQuery, bestResult);

        return bestResult;
    }

    public async Task<NominatimSearchResponseModel?> SearchByQueryAsync(string query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating query");
        var searchQuery = $"search?format=json&q={query}&addressdetails=0";

        _logger.LogInformation("Checking for {@Query} in cacheService", searchQuery);
        if (_cacheService.TryGet<NominatimSearchResponseModel>(searchQuery, out var cachedDto))
        {
            _logger.LogInformation("Returning cached dto {@Dto}", cachedDto);
            return cachedDto;
        }

        _logger.LogInformation("Calling httpClient with {@Query}", searchQuery);
        var result = await _httpClient.GetFromJsonAsync<NominatimSearchResponseModel[]>(
            searchQuery, cancellationToken) ?? Array.Empty<NominatimSearchResponseModel>();

        if (!result.Any())
        {
            _logger.LogWarning("Not found results for {@Query}", searchQuery);
            return null;
        }

        _logger.LogInformation("Found {@Count} results, sorting", result.Count());
        var bestResult = result.OrderByDescending(x => x.importance).First();

        _logger.LogInformation("Caching best result {@BestResult}", bestResult);
        _cacheService.Set(searchQuery, bestResult);

        return bestResult;
    }

    private string ParseParameters(NominatimSearchParametersModel model)
    {
        _logger.LogInformation("Parsing parameters {@Model}", model);

        var searchQuery = new StringBuilder("search?format=json&addressdetails=0");

        var fields = typeof(NominatimSearchParametersModel).GetProperties();

        foreach (var field in fields)
        {
            var value = field.GetValue(model);

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
