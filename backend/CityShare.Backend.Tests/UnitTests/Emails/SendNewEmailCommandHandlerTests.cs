﻿using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Emails.Commands;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Emails;

public class SendNewEmailCommandHandlerTests
{
    private readonly Mock<IEmailRepository> _emailRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly SendNewEmail.Command _command;
    private readonly SendNewEmail.Handler _systemUnderTests;

    public SendNewEmailCommandHandlerTests()
    {
        _emailRepositoryMock = new Mock<IEmailRepository>();

        _emailServiceMock = new Mock<IEmailService>();

        var logger = new Mock<ILogger<SendNewEmail.Handler>>().Object;

        _command = new SendNewEmail.Command(Value.Guid);

        _systemUnderTests = new SendNewEmail.Handler(
            _emailRepositoryMock.Object,
            _emailServiceMock.Object,
            logger);
    }

    [Fact]
    public async Task EmailNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync((Email?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }    
    
    [Fact]
    public async Task EmailStatusNotNew_ShouldReturn_Failure()
    {
        // Arrange
        var newStatusId = Value.Int;
        var email = Value.Email;
        email.StatusId = newStatusId + 1;

        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(email);

        _emailRepositoryMock.Setup(x => x.GetStatusIdAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(newStatusId);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task SendThrowError_ShouldReturn_Failure()
    {
        // Arrange
        var newStatusId = Value.Int;
        var email = Value.Email;
        email.StatusId = newStatusId;

        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(email);

        _emailRepositoryMock.Setup(x => x.GetStatusIdAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(newStatusId);

        _emailServiceMock.Setup(x => x.SendAsync(Any.Email))
            .ThrowsAsync(new Exception());

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
