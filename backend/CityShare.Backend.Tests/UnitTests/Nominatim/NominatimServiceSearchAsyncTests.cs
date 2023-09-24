using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Search;
using CityShare.Backend.Infrastructure.Nominatim;
using CityShare.Backend.Tests.Common;
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

        var logger = new Mock<ILogger<NominatimService>>().Object;

        _systemUnderTests = new NominatimService(
            httpClient, logger);
    }

    [Fact]
    public async Task HttpClient_ShouldBeCalled_WithCorrectQuery()
    {
        // Arrange
        var city = Value.String;
        var dto = new NominatimSearchRequestDto
        {
            City = city
        };

        var parsedQuery = $"search?format=json&addressdetails=0&city={city}";
        
        _mockHttp.Expect($"{Constants.BaseUrl}/{parsedQuery}")
            .Respond(Constants.JsonContentType, Value.JsonEmptyArray);

        // Act
        await _systemUnderTests.SearchAsync(dto);

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task EmptyResult_ShouldReturn_Null()
    {
        // Arrange
        var dto = new NominatimSearchRequestDto();

        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.JsonEmptyArray);

        // Act
        var result = await _systemUnderTests.SearchAsync(dto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CorrectQuery_ShouldReturn_Result()
    {
        // Arrange
        var dto = new NominatimSearchRequestDto();

        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.SerializedArrayWithSearchResult);

        // Act
        var result = await _systemUnderTests.SearchAsync(dto);

        // Assert
        Assert.NotNull(result);
    }
}
