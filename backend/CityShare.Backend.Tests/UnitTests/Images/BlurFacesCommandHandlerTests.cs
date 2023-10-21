using CityShare.Backend.Application.Core.Abstractions.Blobs;
using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Application.Images.Commands;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Images;

public class BlurFacesCommandHandlerTests
{
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IBlobService> _blobServiceMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly BlurFacesCommand _command;
    private readonly BlurFacesCommandHandler _systemUnderTests;

    public BlurFacesCommandHandlerTests()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();

        _blobServiceMock = new Mock<IBlobService>();

        _imageServiceMock = new Mock<IImageService>();

        var logger = new Mock<ILogger<BlurFacesCommandHandler>>().Object;

        _command = new BlurFacesCommand(Value.Guid);

        _systemUnderTests = new BlurFacesCommandHandler(
            _imageRepositoryMock.Object,
            _blobServiceMock.Object,
            _imageServiceMock.Object,
            logger);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        var image = Value.Image;
        image.ShouldBeBlurred = Value.True;
        _imageRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(image);

        var stream = Value.Stream;
        _blobServiceMock.Setup(x => x.ReadFileAsync(Any.String, Any.String, Any.CancellationToken))
            .ReturnsAsync(stream);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CorrectRequest_ShouldSet_ImageAsBlurred()
    {
        // Arrange
        var image = Value.Image;
        image.ShouldBeBlurred = Value.True;
        _imageRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(image);

        var stream = Value.Stream;
        _blobServiceMock.Setup(x => x.ReadFileAsync(Any.String, Any.String, Any.CancellationToken))
            .ReturnsAsync(stream);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        _imageRepositoryMock.Verify(
            x => x.SetIsBlurredAsync(_command.ImageId, Value.True, Value.CancelationToken),
            Times.Once);
    }

    [Fact]
    public async Task ImageNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _imageRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync((Image?)Value.Null);

        var stream = Value.Stream;
        _blobServiceMock.Setup(x => x.ReadFileAsync(Any.String, Any.String, Any.CancellationToken))
            .ReturnsAsync(stream);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(ResultHelper.IsFailureWithErrorCode(result, Errors.NotFound));
    }

    [Fact]
    public async Task ImageShouldNotBeBlurred_ShouldReturn_Failure()
    {
        // Arrange
        var image = Value.Image;
        image.ShouldBeBlurred = Value.False;
        _imageRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(image);

        var stream = Value.Stream;
        _blobServiceMock.Setup(x => x.ReadFileAsync(Any.String, Any.String, Any.CancellationToken))
            .ReturnsAsync(stream);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(ResultHelper.IsFailureWithErrorCode(result, Errors.ForbiddenState));
    }

    [Fact]
    public async Task BlobNotFound_ShouldReturn_Failure()
    {
        // Arrange
        var image = Value.Image;
        image.ShouldBeBlurred = Value.True;
        _imageRepositoryMock.Setup(x => x.GetByIdAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(image);

        var stream = Value.Stream;
        _blobServiceMock.Setup(x => x.ReadFileAsync(Any.String, Any.String, Any.CancellationToken))
            .ReturnsAsync((Stream?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(ResultHelper.IsFailureWithErrorCode(result, Errors.NotFound));
    }
}
