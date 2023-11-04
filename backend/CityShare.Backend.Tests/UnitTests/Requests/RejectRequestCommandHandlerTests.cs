using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Application.Core.Abstractions.Queues;
using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Application.Requests.Commands;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Requests;

public class RejectRequestCommandHandlerTests
{
    private readonly Mock<IRequestRepository> _requestRepositoryMock;
    private readonly Mock<IEmailRepository> _emailRepositoryMock;
    private readonly Mock<IQueueService> _queueServiceMock;
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly RejectRequestCommand _command;
    private readonly RejectRequestCommandHandler _systemUnderTests;

    public RejectRequestCommandHandlerTests()
    {
        _requestRepositoryMock = new Mock<IRequestRepository>();

        _emailRepositoryMock = new Mock<IEmailRepository>();

        _queueServiceMock = new Mock<IQueueService>();

        _imageRepositoryMock = new Mock<IImageRepository>();


        var logger = new Mock<ILogger<RejectRequestCommandHandler>>().Object;

        var options = Options.Create(new CommonSettings());

        _command = new RejectRequestCommand(Value.Guid);

        _systemUnderTests = new RejectRequestCommandHandler(
            _requestRepositoryMock.Object,
            _emailRepositoryMock.Object,
            _queueServiceMock.Object,
            _imageRepositoryMock.Object,
            options,
            logger);
    }

    [Theory]
    [InlineData(RequestTypes.Blur)]
    [InlineData(RequestTypes.Delete)]
    public async Task CorrectRequest_ShouldReturn_Success(string type)
    {
        // Arrange
        var request = Value.Request;
        request.Type = Value.RequestType;
        request.Type.Name = type;
        request.Status = Value.RequestStatus;
        request.Status.Name = RequestStatuses.Pending;
        request.ImageId = Value.Guid;
        request.Author = Value.ApplicationUser;
        request.Author.Email = Value.String;

        _requestRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(request);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RequestNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _requestRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync((Request?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.NotFound);
    }

    [Theory]
    [InlineData(RequestStatuses.Accepted)]
    [InlineData(RequestStatuses.Rejected)]
    public async Task RequestNotPending_ShouldReturn_Failure(string status)
    {
        // Arrange
        var request = Value.Request;
        request.Type = Value.RequestType;
        request.Type.Name = RequestTypes.Blur;
        request.Status = Value.RequestStatus;
        request.Status.Name = status;
        request.ImageId = Value.Guid;
        request.Author = Value.ApplicationUser;
        request.Author.Email = Value.String;

        _requestRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(request);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.RequestNotPending);
    }

    [Fact]
    public async Task RequestHasNoImage_ShouldReturn_Failure()
    {
        // Arrange
        var request = Value.Request;
        request.Type = Value.RequestType;
        request.Type.Name = RequestTypes.Blur;
        request.Status = Value.RequestStatus;
        request.Status.Name = RequestStatuses.Pending;
        request.ImageId = (Guid?)Value.Null;
        request.Author = Value.ApplicationUser;
        request.Author.Email = Value.String;

        _requestRepositoryMock.Setup(x => x.GetByIdWithDetailsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(request);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.RequestHasNoImage);
    }
}
