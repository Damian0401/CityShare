﻿using Castle.Core.Logging;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Events.Queries;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Events;

public class GetEventByIdQueryHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly GetEventByIdQuery _getEventByIdQuery;
    private readonly GetEventByIdQueryHandler _systemUnderTests;

    public GetEventByIdQueryHandlerTests()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();

        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<GetEventByIdQueryHandler>>().Object;

        _getEventByIdQuery = new GetEventByIdQuery(Value.Guid, Value.String);

        _systemUnderTests = new GetEventByIdQueryHandler(
            _eventRepositoryMock.Object,
            _userManagerMockHelper.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task EventFound_ShouldReturn_Success()
    {
        // Arrange
        _eventRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.SearchEventDto);

        // Act
        var result = await _systemUnderTests.Handle(_getEventByIdQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task EventNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _eventRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync((SearchEventDto?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_getEventByIdQuery, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task EventNotFound_ShouldReturn_CorrectErrorCode()
    {
        // Arrange
        _eventRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync((SearchEventDto?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_getEventByIdQuery, Value.CancelationToken);

        // Assert
        Assert.Equal(Errors.NotFound.First(), result.Errors?.First());
    }
}