using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Application.Requests.Commands;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Requests;

public class CreateRequestCommandHandlerTests
{
    private readonly Mock<IRequestRepository> _requestRepositoryMock;
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly UserManagerMockHelper<ApplicationUser> _userManagerMockHelper;
    private readonly CreateRequestCommand _command;
    private readonly CreateRequestCommandHandler _systemUnderTests;

    public CreateRequestCommandHandlerTests()
    {
        _requestRepositoryMock = new Mock<IRequestRepository>();

        _imageRepositoryMock = new Mock<IImageRepository>();

        var mapper = MapperHelper.GetMapper();

        var clock = new Mock<IClock>().Object;

        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        var logger = new Mock<ILogger<CreateRequestCommandHandler>>().Object;

        _command = new CreateRequestCommand(Value.CreateRequestDto, Value.String);

        _systemUnderTests = new CreateRequestCommandHandler(
            _requestRepositoryMock.Object,
            _imageRepositoryMock.Object,
            mapper,
            clock,
            _userManagerMockHelper.Object,
            logger);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = true;
        _userManagerMockHelper.SetupAsync(x => x.FindByIdAsync(Any.String), user);

        _imageRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        _requestRepositoryMock.Setup(x => x.TypeExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task EmailNotConfirmed_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.False;
        _userManagerMockHelper.SetupAsync(x => x.FindByIdAsync(Any.String), user);

        _imageRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        _requestRepositoryMock.Setup(x => x.TypeExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithStatusCode(result, Errors.Forbidden);
    }

    [Fact]
    public async Task EventNotExists_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(x => x.FindByIdAsync(Any.String), user);

        _imageRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.False);

        _requestRepositoryMock.Setup(x => x.TypeExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithStatusCode(result, Errors.ImageNotExists(_command.Request.ImageId));
    }

    [Fact]
    public async Task TypeNotExists_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(x => x.FindByIdAsync(Any.String), user);

        _imageRepositoryMock.Setup(x => x.ExistsAsync(Any.Guid, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        _requestRepositoryMock.Setup(x => x.TypeExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.False);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithStatusCode(result, Errors.RequestTypeNotExists(_command.Request.TypeId));
    }
}
