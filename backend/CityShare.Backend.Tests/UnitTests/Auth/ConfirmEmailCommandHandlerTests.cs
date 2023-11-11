using CityShare.Backend.Application.Auth.Commands;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Helpers;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Domain.Constants;

namespace CityShare.Backend.Tests.UnitTests.Auth;

public class ConfirmEmailCommandHandlerTests
{
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly ConfirmEmailCommand _command;
    private readonly ConfirmEmailCommandHandler _systemUnderTests;

    public ConfirmEmailCommandHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        var logger = new Mock<ILogger<ConfirmEmailCommandHandler>>().Object;

        _command = new ConfirmEmailCommand(
            new EmailConfirmDto(Value.String, Value.String));

        _systemUnderTests = new ConfirmEmailCommandHandler(
            _userManagerMockHelper.Object,
            logger);
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
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

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

        _userManagerMockHelper.SetupAsync(
            x => x.ConfirmEmailAsync(Any.ApplicationUser, Any.String),
            Value.IdentityResultSecceeded);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.Forbidden);
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

        _userManagerMockHelper.SetupAsync(
            x => x.ConfirmEmailAsync(Any.ApplicationUser, Any.String),
            Value.IdentityResultSecceeded);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.EmailAlreadyConfirmed);
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
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }
}
