using CityShare.Backend.Application.Authentication.Commands.ConfirmEmail;
using CityShare.Backend.Application.Core.Dtos.Authentication.ConfirmEmail;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using CityShare.Backend.Tests.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Authentication;

public class ConfirmEmailCommandHandlerTests
{
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly ConfirmEmailCommand _confirmEmailCommand;
    private readonly ConfirmEmailCommandHandler _systemUnderTests;

    public ConfirmEmailCommandHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        var logger = new Mock<ILogger<ConfirmEmailCommandHandler>>().Object;

        _confirmEmailCommand = new ConfirmEmailCommand(
            new EmailConfirmRequestDto(Value.String, Value.String));

        _systemUnderTests = new ConfirmEmailCommandHandler(
            _userManagerMockHelper.GetMockObject(),
            logger);
    }

    [Fact]
    public async Task UserNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            (ApplicationUser?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_confirmEmailCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public async Task EmailAlreadyConfirmed_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = true;

        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        // Act
        var result = await _systemUnderTests.Handle(_confirmEmailCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task ConfirmationFailed_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = false;

        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _userManagerMockHelper.SetupAsync(
            x => x.ConfirmEmailAsync(Any.ApplicationUser, Any.String),
            Value.IdentityResultFailed);

        // Act
        var result = await _systemUnderTests.Handle(_confirmEmailCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = false;

        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _userManagerMockHelper.SetupAsync(
            x => x.ConfirmEmailAsync(Any.ApplicationUser, Any.String),
            Value.IdentityResultSecceeded);

        // Act
        var result = await _systemUnderTests.Handle(_confirmEmailCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
