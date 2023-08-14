using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Emails.Commands.SendNewEmail;
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
    private readonly SendNewEmailCommand _command;
    private readonly SendNewEmailCommandHandler _systemUnderTests;

    public SendNewEmailCommandHandlerTests()
    {
        _emailRepositoryMock = new Mock<IEmailRepository>();

        _emailServiceMock = new Mock<IEmailService>();

        var logger = new Mock<ILogger<SendNewEmailCommandHandler>>().Object;

        _command = new SendNewEmailCommand(Value.Guid);

        _systemUnderTests = new SendNewEmailCommandHandler(
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
    
    [Theory]
    [InlineData(EmailStatuses.Pending)]
    [InlineData(EmailStatuses.Send)]
    [InlineData(EmailStatuses.Error)]
    public async Task EmailStatusNotNew_ShouldReturn_Failure(string status)
    {
        // Arrange
        var email = Value.Email;
        email.Status = status;

        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(email);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task SendThrowError_ShouldReturn_Failure()
    {
        // Arrange
        var email = Value.Email;
        email.Status = EmailStatuses.New;

        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(email);

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
        var email = Value.Email;
        email.Status = EmailStatuses.New;

        _emailRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(email);


        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
