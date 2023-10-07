﻿using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Maps;
using CityShare.Backend.Application.Core.Dtos.Maps;
using CityShare.Backend.Application.Map.Queries;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Maps;

public class MapReverseHandlerTests
{
    private readonly Mock<IMapService> _nominatimServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly ReverseQuery _reverseQuery;
    private readonly ReverseQueryHandler _systemUnderTests;

    public MapReverseHandlerTests()
    {
        _nominatimServiceMock = new Mock<IMapService>();

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<ReverseQueryHandler>>().Object;

        _reverseQuery = new ReverseQuery(Value.Double, Value.Double);

        _systemUnderTests = new ReverseQueryHandler(
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
    public async Task FoundCachedDto_ShouldNot_CallService()
    {
        // Arrange
        var response = Value.AddressDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_reverseQuery, Value.CancelationToken);

        // Assert
        _nominatimServiceMock.Verify(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken), Times.Never);
    }

    [Fact]
    public async Task ResultNotFound_ShouldReturn_Failure()
    {
        // Arrange
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _nominatimServiceMock.Setup(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken))
            .ReturnsAsync((MapReverseResponseDto?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_reverseQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task ResultFound_ShouldReturn_Success()
    {
        // Arrange
        var response = Value.Null;
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
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _nominatimServiceMock.Setup(x => x.ReverseAsync(Any.Double, Any.Double, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimReverseResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_reverseQuery, Value.CancelationToken);

        // Assert
        _cacheServiceMock.Verify(x => x.Set(Any.Object, Any.AddressDto, Any.CacheServiceOptions), Times.Once);
    }
}