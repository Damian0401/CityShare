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

public class MapSearchQueryHandlerTests
{
    private readonly Mock<IMapService> _mapServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly SearchQuery _query;
    private readonly SearchQueryHandler _systemUnderTests;

    public MapSearchQueryHandlerTests()
    {
        _mapServiceMock = new Mock<IMapService>();

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<SearchQueryHandler>>().Object;

        _query = new SearchQuery(Value.String);

        _systemUnderTests = new SearchQueryHandler(
            _mapServiceMock.Object,
            _cacheServiceMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task FoundCachedDto_ShouldReturn_Success()
    {
        // Arrange
        var response = Value.AddressDetailsDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ResultFound_ShouldNot_CallService()
    {
        // Arrange
        var response = Value.AddressDetailsDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        _mapServiceMock.Setup(x => x.SearchByQueryAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimSearchResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        _mapServiceMock.Verify(x => x.SearchByQueryAsync(Any.String, Any.CancellationToken), Times.Never);
    }

    [Fact]
    public async Task ResultNotFound_ShouldReturn_Failure()
    {
        // Arrange
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _mapServiceMock.Setup(x => x.SearchByQueryAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync((MapSearchResponseDto?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.True(ResultHelper.IsFailureWithErrorCode(result, Errors.NotFound));
    }

    [Fact]
    public async Task ResultFound_ShouldReturn_Success()
    {
        // Arrange
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _mapServiceMock.Setup(x => x.SearchByQueryAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimSearchResponseDto);

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

        _mapServiceMock.Setup(x => x.SearchByQueryAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimSearchResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        _cacheServiceMock.Verify(
            x => x.Set(Any.Object, Any.AddressDetailsDto, Any.CacheServiceOptions),
            Times.Once);
    }
}
