using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Reverse;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Search;
using CityShare.Backend.Application.Map.Queries;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Map;

public class MapReverseHandlerTests
{
    private readonly Mock<INominatimService> _nominatimServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Reverse.Query _reverseQuery;
    private readonly Reverse.Handler _systemUnderTests;

    public MapReverseHandlerTests()
    {
        _nominatimServiceMock = new Mock<INominatimService>();

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<Reverse.Handler>>().Object;

        _reverseQuery = new Reverse.Query(Value.Double, Value.Double);

        _systemUnderTests = new Reverse.Handler(
            _nominatimServiceMock.Object,
            _cacheServiceMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task FoundCachedDto_ShouldReturn_Success()
    {
        // Arrange
        var response = Value.AddressDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_reverseQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ResultNotFound_ShouldReturn_Failure()
    {
        // Arrange
        var response = Value.AddressDetailsDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _nominatimServiceMock.Setup(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken))
            .ReturnsAsync((NominatimReverseResponseDto?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_reverseQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task ResultFound_ShouldReturn_Success()
    {
        // Arrange
        var response = Value.AddressDetailsDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _nominatimServiceMock.Setup(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimReverseResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_reverseQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ResultFound_ShouldBeCached()
    {
        // Arrange
        var response = Value.AddressDetailsDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _nominatimServiceMock.Setup(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimReverseResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_reverseQuery, Value.CancelationToken);

        // Assert
        _cacheServiceMock.Verify(x => x.Set(Any.Object, Any.AddressDto, Any.Int), Times.Once);
    }
}
