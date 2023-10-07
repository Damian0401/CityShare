﻿using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Application.Emails.Commands;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Common;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Emails;

public class SendPendingEmailCommandHandlerTests
{
    private readonly Mock<IEmailRepository> _emailRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly SendPendingEmailCommand _command;
    private readonly SendPendingEmailCommandHandler _systemUnderTests;

    public SendPendingEmailCommandHandlerTests()
    {
        _emailRepositoryMock = new Mock<IEmailRepository>();

        _emailServiceMock = new Mock<IEmailService>();

        var click = new Mock<IClock>();

        var logger = new Mock<ILogger<SendPendingEmailCommandHandler>>().Object;

        _command = new SendPendingEmailCommand(Value.Guid);

        _systemUnderTests = new SendPendingEmailCommandHandler(
            _emailRepositoryMock.Object,
            _emailServiceMock.Object,
            click.Object,
            logger);
    }

    [Fact]
    public async Task EmailNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync((Email?)Value.Null);

        var newStatusId = Value.Int;
        _emailRepositoryMock.Setup(x => x.GetStatusIdAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(newStatusId);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }    
    
    [Fact]
    public async Task EmailStatusNotPending_ShouldReturn_Failure()
    {
        // Arrange
        var pendingStatusId = Value.Int;
        var email = Value.Email;
        email.StatusId = pendingStatusId + 1;

        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(email);

        _emailRepositoryMock.Setup(x => x.GetStatusIdAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(pendingStatusId);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        var newStatusId = Value.Int;
        var email = Value.Email;
        email.StatusId = newStatusId;

        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(email);

        _emailRepositoryMock.Setup(x => x.GetStatusIdAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(newStatusId);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}