﻿using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Events.Commands;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Events;

public class UpdateEventLikesCommandHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly UpdateEventLikesCommand _command;
    private readonly UpdateEventLikesCommandHandler _systemUnderTests;

    public UpdateEventLikesCommandHandlerTests()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<UpdateEventLikesCommandHandler>>().Object;

        _command = new UpdateEventLikesCommand(
            Value.Guid,
            Value.String);

        _systemUnderTests = new UpdateEventLikesCommandHandler(
            _eventRepositoryMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task EventNotLiked_ShouldCall_AddMethod()
    {
        // Arrange
        _eventRepositoryMock.Setup(x => x.IsLikedByUserAsync(Any.Guid, Any.String, Any.CancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        _eventRepositoryMock.Verify(x => x.AddLikeAsync(Any.Like, Any.CancellationToken), Times.Once);
        _eventRepositoryMock.Verify(x => x.RemoveLikeAsync(_command.EventId, _command.AuthorId, Any.CancellationToken), Times.Never);
    }

    [Fact]
    public async Task EventAlreadyLiked_ShouldCall_RemoveMethod()
    {
        // Arrange
        _eventRepositoryMock.Setup(x => x.IsLikedByUserAsync(Any.Guid, Any.String, Any.CancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        _eventRepositoryMock.Verify(x => x.AddLikeAsync(Any.Like, Any.CancellationToken), Times.Never);
        _eventRepositoryMock.Verify(x => x.RemoveLikeAsync(_command.EventId, _command.AuthorId, Any.CancellationToken), Times.Once);
    }
}
