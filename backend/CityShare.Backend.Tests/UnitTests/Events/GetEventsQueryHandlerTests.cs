﻿using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Events.Queries;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Events;

public class GetEventsQueryHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly EventSearchQueryDto _request;
    private readonly GetEventsQuery _query;
    private readonly GetEventsQueryHandler _systemUnderTests;

    public GetEventsQueryHandlerTests()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<GetEventsQueryHandler>>().Object;

        _request = new EventSearchQueryDto();

        _query = new GetEventsQuery(_request, Value.String);

        _systemUnderTests = new GetEventsQueryHandler(
            _eventRepositoryMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task PageSizeNotSet_ShouldReturn_MaxPageSize()
    {
        // Arrange
        _request.PageSize = (int?)Value.Null;

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.Equal(Domain.Constants.Constants.MaxEventPageSize, result.Value.PageSize);
    }

    [Fact]
    public async Task PageNumberNotSet_ShouldReturn_FirstPage()
    {
        // Arrange
        _request.PageNumber = (int?)Value.Null;

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.Equal(Value.One, result.Value.PageNumber);
    }

    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(10, 15, 2)]
    [InlineData(5, 100, 20)]
    public async Task CalculatedTotalPages_ShouldBe_Correct(int pageSize, int totalCount, int expectedTotalPages)
    {
        // Arrange
        _request.PageSize = pageSize;

        _eventRepositoryMock.Setup(x => x.GetByQueryAsync(Any.EventQueryDto, Any.CancellationToken))
            .ReturnsAsync((Value.SearchEventDtos, totalCount));

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.Equal(expectedTotalPages, result.Value.TotalPages);
    }
}
