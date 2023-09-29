using CityShare.Backend.Infrastructure.Nominatim;
using CityShare.Backend.Tests.Other.Common;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System.Globalization;

namespace CityShare.Backend.Tests.UnitTests.Nominatim;

public class NominatimServiceReverseAsyncTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly NominatimService _systemUnderTests;

    public NominatimServiceReverseAsyncTests()
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
        var x = Value.Double;
        var y = Value.Double;
        var parsedQuery = $"reverse?format=json&zoom=18&addressdetails=0" +
            $"&lat={x.ToString(CultureInfo.InvariantCulture)}&lon={y.ToString(CultureInfo.InvariantCulture)}";

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

        _mockHttp.Fallback
            .Respond(Constants.JsonContentType, Value.SerializedReverseResponseDto);

        // Act
        var result = await _systemUnderTests.ReverseAsync(x, y);

        // Assert
        Assert.NotNull(result);
    }
}
