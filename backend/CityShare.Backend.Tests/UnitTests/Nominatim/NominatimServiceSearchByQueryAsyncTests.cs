﻿using CityShare.Backend.Infrastructure.Nominatim;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace CityShare.Backend.Tests.UnitTests.Nominatim;

public class NominatimServiceSearchByQueryAsyncTests
{
    private readonly ILogger<NominatimService> _logger;
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly NominatimService _systemUnderTests;

    public NominatimServiceSearchByQueryAsyncTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(Constants.BaseUrl);

        var mapper = MapperHelper.GetMapper();

        _logger = new Mock<ILogger<NominatimService>>().Object;

        _systemUnderTests = new NominatimService(
            httpClient, mapper, _logger);
    }

    [Fact]
    public async Task HttpClient_ShouldBeCalled_WithCorrectQuery()
    {
        // Arrange
        var query = Value.String;
        var parsedQuery = $"search?format=json&q={query}";

        _mockHttp.Expect($"{Constants.BaseUrl}/{parsedQuery}")
            .Respond(Constants.JsonContentType, Value.JsonEmptyArray);

        // Act
        await _systemUnderTests.SearchByQueryAsync(query);

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task EmptyResult_ShouldReturn_Null()
    {
        // Arrange
        var query = Value.String;
        
        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.JsonEmptyArray);

        // Act
        var result = await _systemUnderTests.SearchByQueryAsync(query);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CorrectQuery_ShouldReturn_Result()
    {
        // Arrange
        var query = Value.String;

        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.SerializedArrayWithSearchResult);

        // Act
        var result = await _systemUnderTests.SearchByQueryAsync(query);

        // Assert
        Assert.NotNull(result);
    }
}
