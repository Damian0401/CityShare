using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Models.Nominatim.Search;
using CityShare.Backend.Infrastructure.Nominatim;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace CityShare.Backend.Tests.UnitTests.Nominatim;

public class NominatimServiceSearchByQueryAsyncTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private Mock<ICacheService> _cacheServiceMock;
    private readonly NominatimService _systemUnderTests;

    public NominatimServiceSearchByQueryAsyncTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(Constants.BaseUrl);

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<NominatimService>>().Object;

        _systemUnderTests = new NominatimService(
            httpClient, _cacheServiceMock.Object, mapper, logger);
    }

    [Fact]
    public async Task FoundCachedDto_ShouldReturn_CachedDto()
    {
        // Arrange
        var city = Value.String;
        var model = new SearchParametersModel
        {
            City = city
        };
        var parsedQuery = Value.String;

        var dto = Value.SearchDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.String, out dto)).Returns(true);

        // Act
        var result = await _systemUnderTests.SearchAsync(model);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task HttpClient_ShouldBeCalled_WithCorrectQuery()
    {
        // Arrange
        var query = Value.String;
        var parsedQuery = $"search?format=json&addressdetails=0&q={query}";

        var dto = Value.SearchDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.String, out dto)).Returns(false);

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

        var dto = Value.SearchDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.String, out dto)).Returns(false);

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

        var dto = Value.SearchDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.String, out dto)).Returns(false);

        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.SerializedArrayWithSearchResult);

        // Act
        var result = await _systemUnderTests.SearchByQueryAsync(query);

        // Assert
        Assert.NotNull(result);
    }
}
