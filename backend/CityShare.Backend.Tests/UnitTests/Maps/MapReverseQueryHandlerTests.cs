using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Maps;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Application.Maps.Queries;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Maps;

public class MapReverseHandlerTests
{
    private readonly Mock<IMapService> _mapServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly ReverseQuery _query;
    private readonly ReverseQueryHandler _systemUnderTests;

    public MapReverseHandlerTests()
    {
        _mapServiceMock = new Mock<IMapService>();

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<ReverseQueryHandler>>().Object;

        _query = new ReverseQuery(Value.Double, Value.Double);

        _systemUnderTests = new ReverseQueryHandler(
            _mapServiceMock.Object,
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
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task FoundCachedDto_ShouldNot_CallService()
    {
        // Arrange
        var response = Value.AddressDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        _mapServiceMock.Verify(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken), Times.Never);
    }

    [Fact]
    public async Task ResultNotFound_ShouldReturn_Failure()
    {
        // Arrange
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _mapServiceMock.Setup(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken))
            .ReturnsAsync((MapReverseResponseDto?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.NotFound);
    }

    [Fact]
    public async Task ResultFound_ShouldReturn_Success()
    {
        // Arrange
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _mapServiceMock.Setup(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimReverseResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ResultFound_ShouldBeCached()
    {
        // Arrange
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _mapServiceMock.Setup(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimReverseResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        _cacheServiceMock.Verify(x => x.Set(Any.Object, Any.AddressDto, Any.CacheServiceOptions), Times.Once);
    }
}
