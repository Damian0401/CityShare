using CityShare.Backend.Application.Core.Abstractions.Cities;
using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Application.Requests.Queries;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Requests;

public class GetPendingRequestsByCityIdQueryHandlerTests
{ 
    private readonly Mock<IRequestRepository> _requestRepositoryMock;
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly GetPendingRequestsByCityIdQuery _query;
    private readonly GetPendingRequestsByCityIdQueryHandler _systemUnderTests;

    public GetPendingRequestsByCityIdQueryHandlerTests()
    {
        _requestRepositoryMock = new Mock<IRequestRepository>();

        _cityRepositoryMock = new Mock<ICityRepository>();

        var logger = new Mock<ILogger<GetPendingRequestsByCityIdQueryHandler>>().Object;

        var mapper = MapperHelper.GetMapper();

        _query = new GetPendingRequestsByCityIdQuery(Value.Int);

        _systemUnderTests = new GetPendingRequestsByCityIdQueryHandler(
            _requestRepositoryMock.Object,
            _cityRepositoryMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        _cityRepositoryMock.Setup(x => x.ExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        _requestRepositoryMock.Setup(x => x.GetPendingByCityIdWithDetailsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.Requests);

        // Act
        var result = await _systemUnderTests.Handle(_query, Any.CancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CityNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _cityRepositoryMock.Setup(x => x.ExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.False);

        _requestRepositoryMock.Setup(x => x.GetPendingByCityIdWithDetailsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.Requests);

        // Act
        var result = await _systemUnderTests.Handle(_query, Any.CancellationToken);

        // Assert
        AssertHelper.FailureWithStatusCode(result, Errors.CityNotExists(_query.CityId));
    }
}
