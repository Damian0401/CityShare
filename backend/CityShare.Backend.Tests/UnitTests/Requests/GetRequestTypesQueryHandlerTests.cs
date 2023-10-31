using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Application.Requests.Queries;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Requests;

public class GetRequestTypesQueryHandlerTests
{
    private readonly Mock<IRequestRepository> _requestRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetRequestTypesQuery _query;
    private readonly GetRequestTypesQueryHandler _systemUnderTests;

    public GetRequestTypesQueryHandlerTests()
    {
        _requestRepositoryMock = new Mock<IRequestRepository>();

        _cacheServiceMock = new Mock<ICacheService>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<GetRequestTypesQueryHandler>>().Object;

        _query = new GetRequestTypesQuery();

        _systemUnderTests = new GetRequestTypesQueryHandler(_requestRepositoryMock.Object, 
            _cacheServiceMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task FoundCachedDtos_ShouldReturn_CachedDtos()
    {
        // Arrange
        var response = Value.RequestTypeDtos;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.Same(response, result.Value);
    }

    [Fact]
    public async Task FoundCachedDtos_ShouldNot_CallRepository()
    {
        // Arrange
        var response = Value.RequestTypeDtos;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(true);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        _requestRepositoryMock.Verify(x => x.GetTypesAsync(Value.CancelationToken), Times.Never);
    }

    [Fact]
    public async Task NotFoundCachedDtos_Should_CallRepository()
    {
        // Arrange
        var response = Value.RequestTypeDtos;
        _cacheServiceMock.Setup(x => x.TryGet(Any.Object, out response)).Returns(false);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        _requestRepositoryMock.Verify(x => x.GetTypesAsync(Value.CancelationToken), Times.Once);
    }
}
