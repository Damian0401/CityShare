using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Emails.Commands;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Emails;

public class SendPendingEmailsCommandHandlerTests
{
    private readonly Mock<IEmailRepository> _emailRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly SendPendingEmailsCommand _command;
    private readonly SendPendingEmailsCommandHandler _systemUnderTests;

    public SendPendingEmailsCommandHandlerTests()
    {
        _emailRepositoryMock = new Mock<IEmailRepository>();

        _emailServiceMock = new Mock<IEmailService>();

        var logger = new Mock<ILogger<SendPendingEmailsCommandHandler>>().Object;

        _command = new SendPendingEmailsCommand();

        _systemUnderTests = new SendPendingEmailsCommandHandler(
            _emailRepositoryMock.Object,
            _emailServiceMock.Object,
            logger);
    }

    [Fact]
    public async Task NotFoundPendingEmails_ShouldReturn_CorrectResponse()
    {
        // Arrange
        _emailRepositoryMock.Setup(x => x.GetAllWithStatusAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Enumerable.Empty<Email>());

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.Equal(0, result.Value.ErrorEmails);
        Assert.Equal(0, result.Value.NotSentEmails);
        Assert.Equal(0, result.Value.SentEmails);
    }

    [Fact]
    public async Task RetryNumberReached_ShouldReturn_CorrectResponse()
    {
        // Arrange
        var priorityId = Value.Int;
        var retryNumber = Value.Int;

        var priority = Value.EmailPriority;
        priority.Id = priorityId;
        priority.RetryNumber = retryNumber;

        var email = Value.Email;
        email.PrirorityId = priorityId;
        email.TryCount = retryNumber;

        _emailRepositoryMock.Setup(x => x.GetAllWithStatusAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(new[] { email });

        _emailRepositoryMock.Setup(x => x.GetAllPrioritiesAsync(Any.CancellationToken))
            .ReturnsAsync(new[] { priority });

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.Equal(1, result.Value.ErrorEmails);
        Assert.Equal(0, result.Value.NotSentEmails);
        Assert.Equal(0, result.Value.SentEmails);
    }

    [Fact]
    public async Task SendThrowError_ShouldReturn_CorrectResponse()
    {
        // Arrange
        var priorityId = Value.Int;
        var retryNumber = Value.Int;

        var priority = Value.EmailPriority;
        priority.Id = priorityId;
        priority.RetryNumber = retryNumber;

        var email = Value.Email;
        email.PrirorityId = priorityId;
        email.TryCount = retryNumber - 1;

        _emailRepositoryMock.Setup(x => x.GetAllWithStatusAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(new[] { email });

        _emailRepositoryMock.Setup(x => x.GetAllPrioritiesAsync(Any.CancellationToken))
            .ReturnsAsync(new[] { priority });

        _emailServiceMock.Setup(x => x.SendAsync(Any.Email)).ThrowsAsync(new Exception());

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.Equal(0, result.Value.ErrorEmails);
        Assert.Equal(1, result.Value.NotSentEmails);
        Assert.Equal(0, result.Value.SentEmails);
    }

    [Fact]
    public async Task SendEmail_ShouldReturn_CorrectResponse()
    {
        // Arrange
        var priorityId = Value.Int;
        var retryNumber = Value.Int;

        var priority = Value.EmailPriority;
        priority.Id = priorityId;
        priority.RetryNumber = retryNumber;

        var email = Value.Email;
        email.PrirorityId = priorityId;
        email.TryCount = retryNumber - 1;

        _emailRepositoryMock.Setup(x => x.GetAllWithStatusAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(new[] { email });

        _emailRepositoryMock.Setup(x => x.GetAllPrioritiesAsync(Any.CancellationToken))
            .ReturnsAsync(new[] { priority });

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.Equal(0, result.Value.ErrorEmails);
        Assert.Equal(0, result.Value.NotSentEmails);
        Assert.Equal(1, result.Value.SentEmails);
    }
}

