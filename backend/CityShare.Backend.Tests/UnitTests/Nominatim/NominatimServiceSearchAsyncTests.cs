﻿using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using CityShare.Backend.Infrastructure.Nominatim;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace CityShare.Backend.Tests.UnitTests.Nominatim;

public class NominatimServiceSearchAsyncTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly NominatimService _systemUnderTests;

    public NominatimServiceSearchAsyncTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(Constants.BaseUrl);

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<NominatimService>>().Object;

        _systemUnderTests = new NominatimService(
            httpClient, mapper, logger);
    }

    [Fact]
    public async Task HttpClient_ShouldBeCalled_WithCorrectQuery()
    {
        // Arrange
        var city = Value.String;
        var model = new SearchParametersModel
        {
            City = city
        };
        var parsedQuery = $"search?format=json&addressdetails=1&city={city}";

        _mockHttp.Expect($"{Constants.BaseUrl}/{parsedQuery}")
            .Respond(Constants.JsonContentType, Value.JsonEmptyArray);

        // Act
        await _systemUnderTests.SearchAsync(model);

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task EmptyResult_ShouldReturn_Null()
    {
        // Arrange
        var model = new SearchParametersModel();
        
        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.JsonEmptyArray);

        // Act
        var result = await _systemUnderTests.SearchAsync(model);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CorrectQuery_ShouldReturn_Result()
    {
        // Arrange
        var model = new SearchParametersModel();

        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.SerializedArrayWithSearchResult);

        // Act
        var result = await _systemUnderTests.SearchAsync(model);

        // Assert
        Assert.NotNull(result);
    }
}
