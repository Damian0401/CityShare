using CityShare.Backend.Application.Categories.Queries;
using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Categories;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Categories;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllCategoriesQuery _getAllCategoriesQuery;
    private readonly GetAllCategoriesQueryHandler _systemUnderTests;

    public GetAllCategoriesQueryHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<GetAllCategoriesQueryHandler>>().Object;

        _getAllCategoriesQuery = new GetAllCategoriesQuery();

        _systemUnderTests = new GetAllCategoriesQueryHandler(
            _categoryRepositoryMock.Object,
            _cacheServiceMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task FoundCachedDtos_ShouldReturn_CachedDtos()
    {
        // Arrange
        var response = Value.CategoryDtos;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_getAllCategoriesQuery, Value.CancelationToken);

        // Assert
        Assert.Same(response, result.Value);
    }

    [Fact]
    public async Task FoundCachedDtos_ShouldNot_CallRepository()
    {
        // Arrange
        var response = Value.CategoryDtos;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_getAllCategoriesQuery, Value.CancelationToken);

        // Assert
        _categoryRepositoryMock.Verify(x => x.GetAllAsync(Any.CancellationToken), Times.Never);
    }

    [Fact]
    public async Task ResultFound_ShouldBeCached()
    {
        // Arrange
        var response = Value.Null;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        _categoryRepositoryMock.Setup(x => x.GetAllAsync(Any.CancellationToken))
            .ReturnsAsync(Value.Categories);

        // Act
        var result = await _systemUnderTests.Handle(_getAllCategoriesQuery, Value.CancelationToken);

        // Assert
        _cacheServiceMock.Verify(
            x => x.Set(Any.Object, Any.CategoryDtos, Any.CacheServiceOptions),
            Times.Once);
    }
}
