using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using System.Net.Http.Json;
using System.Text;

namespace CityShare.Backend.Infrastructure.Nominatim;

public class NominatimService : INominatimService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public NominatimService(HttpClient httpClient, IMapper mapper)
    {
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<SearchDto?> SearchAsync(SearchParametersModel model, CancellationToken cancellationToken = default)
    {
        var searchQuery = ParseParameters(model);

        var result = await _httpClient.GetFromJsonAsync<SearchResultModel[]>(
            searchQuery.ToString(), cancellationToken);

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

    private static string ParseParameters(SearchParametersModel model)
    {
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
