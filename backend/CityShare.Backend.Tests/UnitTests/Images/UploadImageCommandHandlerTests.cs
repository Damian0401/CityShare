using CityShare.Backend.Application.Core.Abstractions.Blobs;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Application.Core.Abstractions.Queues;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Helpers;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Constants = CityShare.Backend.Domain.Constants.Constants;
using CityShare.Backend.Application.Images.Commands;

namespace CityShare.Backend.Tests.UnitTests.Images;

public class UploadImageCommandHandlerTests
{
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IBlobService> _blobServiceMock;
    private readonly Mock<IQueueService> _queueServiceMock;
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly Mock<IFormFile> _image;
    private readonly UploadImageCommand _command;
    private readonly UploadImageCommandHandler _systemUnderTests;

    public UploadImageCommandHandlerTests()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();

        _eventRepositoryMock = new Mock<IEventRepository>();

        _blobServiceMock = new Mock<IBlobService>();

        _queueServiceMock = new Mock<IQueueService>();

        _userManagerMockHelper = _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        var logger = new Mock<ILogger<UploadImageCommandHandler>>().Object;

        _image = new Mock<IFormFile>();

        _command = new UploadImageCommand
        {
            Image = _image.Object,
            EventId = Value.Guid,
            UserId = Value.String
        };

        _systemUnderTests = new UploadImageCommandHandler(
            _imageRepositoryMock.Object,
            _eventRepositoryMock.Object,
            _blobServiceMock.Object,
            _queueServiceMock.Object,
            _userManagerMockHelper.Object,
            logger);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _eventRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        _eventRepositoryMock.Setup(x => x.GetImagesNumberAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.Zero);

        _image.Setup(x => x.Length)
            .Returns(Constants.ImageSizeLimitInMB * FileSizes.MB / 2);

        // Act
        var result = await _systemUnderTests.Handle(_command, Any.CancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ImageSizeToBig_ShouldReturn_Failures()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _eventRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        _eventRepositoryMock.Setup(x => x.GetImagesNumberAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.Zero);

        _image.Setup(x => x.Length)
            .Returns(Constants.ImageSizeLimitInMB * FileSizes.MB + 1);

        // Act
        var result = await _systemUnderTests.Handle(_command, Any.CancellationToken);

        // Assert
        AssertHelper.FailureWithStatusCode(result, Errors.ImageSizeLimit);
    }

    [Fact]
    public async Task ReachedImagesLimit_ShouldReturn_Failures()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _eventRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        _eventRepositoryMock.Setup(x => x.GetImagesNumberAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Constants.MaxEventImagesNumber);

        _image.Setup(x => x.Length)
            .Returns(Constants.ImageSizeLimitInMB * FileSizes.MB / 2);

        // Act
        var result = await _systemUnderTests.Handle(_command, Any.CancellationToken);

        // Assert
        AssertHelper.FailureWithStatusCode(result, Errors.MaxImagesNumber);
    }

    [Fact]
    public async Task EventNotExists_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _eventRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.False);

        _eventRepositoryMock.Setup(x => x.GetImagesNumberAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.Zero);

        _image.Setup(x => x.Length)
            .Returns(Constants.ImageSizeLimitInMB * FileSizes.MB / 2);

        // Act
        var result = await _systemUnderTests.Handle(_command, Any.CancellationToken);

        // Assert
        AssertHelper.FailureWithStatusCode(result, Errors.NotFound);
    }

    [Fact]
    public async Task EmailNotConfirmed_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.False;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _eventRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        _eventRepositoryMock.Setup(x => x.GetImagesNumberAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.Zero);

        _image.Setup(x => x.Length)
            .Returns(Constants.ImageSizeLimitInMB * FileSizes.MB / 2);

        // Act
        var result = await _systemUnderTests.Handle(_command, Any.CancellationToken);

        // Assert
        AssertHelper.FailureWithStatusCode(result, Errors.Forbidden);
    }
}
