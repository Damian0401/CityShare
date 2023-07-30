using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;

namespace CityShare.Backend.Infrastructure.Nominatim;

public class NominatimService : INominatimService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ILogger<NominatimService> _logger;

    public NominatimService(
        HttpClient httpClient, 
        IMapper mapper, 
        ILogger<NominatimService> logger)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SearchDto?> SearchAsync(SearchParametersModel model, CancellationToken cancellationToken = default)
    {
        var searchQuery = ParseParameters(model);

        _logger.LogInformation("Calling httpClient with {@Query}", searchQuery);
        var result = await _httpClient.GetFromJsonAsync<SearchResultModel[]>(
            searchQuery, cancellationToken);

        if (result is null || !result.Any())
        {
            return null;
        }

        var bestResult = result.OrderByDescending(x => x.importance).First();

        var dto = _mapper.Map<SearchDto>(bestResult);

        return dto;
    }

    public async Task<SearchDto?> SearchByQueryAsync(string query, CancellationToken cancellationToken = default)
    {
        var searchQuery = $"search?format=json&q={query}";

        _logger.LogInformation("Calling httpClient with {@Query}", searchQuery);
        var result = await _httpClient.GetFromJsonAsync<SearchResultModel[]>(
            searchQuery, cancellationToken) ?? Array.Empty<SearchResultModel>();

        if (!result.Any())
        {
            _logger.LogWarning("Not found results for {@Query}", searchQuery);
            return null;
        }

        _logger.LogInformation("Found {@Count} results, sorting", result.Count());
        var bestResult = result.OrderByDescending(x => x.importance).First();

        _logger.LogInformation("Mapping best result {@Result}", bestResult);
        var dto = _mapper.Map<SearchDto>(bestResult);

        return dto;
    }

    private string ParseParameters(SearchParametersModel model)
    {
        _logger.LogInformation("Parsing parameters {@Model}", model);

        var searchQuery = new StringBuilder("search?format=json");

        var fields = typeof(SearchParametersModel).GetProperties();

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
