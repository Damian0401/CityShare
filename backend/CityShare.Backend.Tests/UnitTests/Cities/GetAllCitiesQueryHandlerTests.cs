﻿using CityShare.Backend.Application.Categories.Queries;
using CityShare.Backend.Application.Cities.Queries;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Categories;
using CityShare.Backend.Application.Core.Abstractions.Cities;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Cities;

public class GetAllCitiesQueryHandlerTests
{
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllCitiesQuery _getAllCitiesQuery;
    private readonly GetAllCitiesQueryHandler _systemUnderTests;

    public GetAllCitiesQueryHandlerTests()
    {
        _cityRepositoryMock = new Mock<ICityRepository>();

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<GetAllCitiesQueryHandler>>().Object;

        _getAllCitiesQuery = new GetAllCitiesQuery();

        _systemUnderTests = new GetAllCitiesQueryHandler(
            _cityRepositoryMock.Object,
            _cacheServiceMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task FoundCachedDtos_ShouldReturn_CachedDtos()
    {
        // Arrange
        var response = Value.CityDtos;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_getAllCitiesQuery, Value.CancelationToken);

        // Assert
        Assert.Same(response, result.Value);
    }

    [Fact]
    public async Task FoundCachedDtos_ShouldNot_CallRepository()
    {
        // Arrange
        var response = Value.CityDtos;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_getAllCitiesQuery, Value.CancelationToken);

        // Assert
        _cityRepositoryMock.Verify(x => x.GetAllWithDetailsAsync(Any.CancellationToken), Times.Never);
    }

    [Fact]
    public async Task ResultFound_ShouldBeCached()
    {
        // Arrange
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _cityRepositoryMock.Setup(x => x.GetAllWithDetailsAsync(Any.CancellationToken))
            .ReturnsAsync(Value.Cities);

        // Act
        var result = await _systemUnderTests.Handle(_getAllCitiesQuery, Value.CancelationToken);

        // Assert
        _cacheServiceMock.Verify(
            x => x.Set(Any.Object, Any.CityDtos, Any.CacheServiceOptions),
            Times.Once);
    }
}
