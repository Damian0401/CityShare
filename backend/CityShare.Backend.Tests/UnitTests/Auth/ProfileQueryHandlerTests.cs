using CityShare.Backend.Application.Auth.Queries;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Likes;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Auth;

public class ProfileQueryHandlerTests
{
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<ILikeRepository> _likeRepositoryMock;
    private readonly ProfileQuery _query;
    private readonly ProfileQueryHandler _systemUnderTests;

    public ProfileQueryHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        _eventRepositoryMock = new Mock<IEventRepository>();

        _likeRepositoryMock = new Mock<ILikeRepository>();

        var logger = new Mock<ILogger<ProfileQueryHandler>>().Object;

        _query = new ProfileQuery(Value.String);

        _systemUnderTests = new ProfileQueryHandler(
            _userManagerMockHelper.Object,
            _eventRepositoryMock.Object,
            _likeRepositoryMock.Object,
            logger);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            Value.ApplicationUser);

        _eventRepositoryMock.Setup(x => x.GetCreatedCountAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.Int);

        _likeRepositoryMock.Setup(x => x.GetGivenCountAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.Int);

        _likeRepositoryMock.Setup(x => x.GetReceivedCountAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.Int);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UserNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            (ApplicationUser?)Value.Null);

        _eventRepositoryMock.Setup(x => x.GetCreatedCountAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.Int);

        _likeRepositoryMock.Setup(x => x.GetGivenCountAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.Int);

        _likeRepositoryMock.Setup(x => x.GetReceivedCountAsync(Any.String, Any.CancellationToken))
            .ReturnsAsync(Value.Int);

        // Act
        var result = await _systemUnderTests.Handle(_query, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.NotFound);
    }
}
