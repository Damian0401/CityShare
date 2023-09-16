using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos.Nominatim.Search;
using CityShare.Backend.Application.Map.Queries;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Map;

public class MapSearchQueryHandlerTests
{
    private readonly Mock<INominatimService> _nominatimServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly SearchQuery _searchQuery;
    private readonly SearchQueryHandler _systemUnderTests;

    public MapSearchQueryHandlerTests()
    {
        _nominatimServiceMock = new Mock<INominatimService>();

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<SearchQueryHandler>>().Object;

        _searchQuery = new SearchQuery(Value.String);

        _systemUnderTests = new SearchQueryHandler(
            _nominatimServiceMock.Object,
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
        var result = await _systemUnderTests.Handle(_searchQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ResultNotFound_ShouldReturn_Failure()
    {
        // Arrange
        var response = Value.AddressDetailsDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _nominatimServiceMock.Setup(x => x.SearchByQueryAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync((NominatimSearchResponseDto?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_searchQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task ResultFound_ShouldReturn_Success()
    {
        // Arrange
        var response = Value.AddressDetailsDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _nominatimServiceMock.Setup(x => x.SearchByQueryAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimSearchResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_searchQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ResultFound_ShouldBeCached()
    {
        // Arrange
        var response = Value.AddressDetailsDto;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _nominatimServiceMock.Setup(x => x.SearchByQueryAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.NominatimSearchResponseDto);

        // Act
        var result = await _systemUnderTests.Handle(_searchQuery, Value.CancelationToken);


        // Assert
        _cacheServiceMock.Verify(
            x => x.Set(Any.Object, Any.AddressDetailsDto, Any.Int),
            Times.Once);
    }
}
