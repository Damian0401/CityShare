using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Infrastructure.Nominatim;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System.Globalization;

namespace CityShare.Backend.Tests.UnitTests.Nominatim;

public class NominatimServiceReverseAsyncTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private Mock<ICacheService> _cacheServiceMock;
    private readonly NominatimService _systemUnderTests;

    public NominatimServiceReverseAsyncTests()
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
    public async Task FoundCachedResponse_ShouldReturn_CachedResponse()
    {
        // Arrange
        var x = Value.Double;
        var y = Value.Double;
        
        var dto = Value.MapReverseResponseModel;
        _cacheServiceMock.Setup(x => x.TryGet(Any.String, out dto)).Returns(true);

        // Act
        var result = await _systemUnderTests.ReverseAsync(x, y);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task HttpClient_ShouldBeCalled_WithCorrectQuery()
    {
        // Arrange
        var x = Value.Double;
        var y = Value.Double;
        var parsedQuery = $"reverse?format=json&zoom=18&addressdetails=0" +
            $"&lat={x.ToString(CultureInfo.InvariantCulture)}&lon={y.ToString(CultureInfo.InvariantCulture)}";

        var dto = Value.MapReverseResponseModel;
        _cacheServiceMock.Setup(x => x.TryGet(Any.String, out dto)).Returns(false);

        _mockHttp.Expect($"{Constants.BaseUrl}/{parsedQuery}")
            .Respond(Constants.JsonContentType, Value.SerializedNull);

        // Act
        await _systemUnderTests.ReverseAsync(x, y);

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task EmptyResult_ShouldReturn_Null()
    {
        // Arrange
        var x = Value.Double;
        var y = Value.Double;

        var dto = Value.MapReverseResponseModel;
        _cacheServiceMock.Setup(x => x.TryGet(Any.String, out dto)).Returns(false);

        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.SerializedNull);

        // Act
        var result = await _systemUnderTests.ReverseAsync(x, y);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CorrectQuery_ShouldReturn_Result()
    {
        // Arrange
        var x = Value.Double;
        var y = Value.Double;

        var dto = Value.MapReverseResponseModel;
        _cacheServiceMock.Setup(x => x.TryGet(Any.String, out dto)).Returns(false);

        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.SerializedReverseResult);

        // Act
        var result = await _systemUnderTests.ReverseAsync(x, y);

        // Assert
        Assert.NotNull(result);
    }
}
