﻿using CityShare.Backend.Application.Auth.Commands;
using CityShare.Backend.Application.Core.Abstractions.Auth;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Auth;

public class RefreshCommandHandlerTests
{
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly RefreshCommand _command;
    private readonly RefreshCommandHandler _systemUnderTests;

    public RefreshCommandHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        _jwtProviderMock = new Mock<IJwtProvider>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<RefreshCommandHandler>>().Object;

        var request = new RefreshDto(Value.String);

        _command = new RefreshCommand(request, Value.String);

        _systemUnderTests = new RefreshCommandHandler(
            _userManagerMockHelper.Object,
            _jwtProviderMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        _jwtProviderMock.Setup(x => x.GetEmailFromToken(Any.String))
            .Returns(Value.String);

        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            Value.ApplicationUser);

        _userManagerMockHelper.SetupAsync(
            x => x.VerifyUserTokenAsync(Any.ApplicationUser, Any.String, Any.String, Any.String),
            Value.True);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task EmailNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _jwtProviderMock.Setup(x => x.GetEmailFromToken(Any.String))
            .Returns((string?)Value.Null);

        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            Value.ApplicationUser);

        _userManagerMockHelper.SetupAsync(
            x => x.VerifyUserTokenAsync(Any.ApplicationUser, Any.String, Any.String, Any.String),
            Value.True);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.InvalidCredentials);
    }

    [Fact]
    public async Task UserNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _jwtProviderMock.Setup(x => x.GetEmailFromToken(Any.String))
            .Returns(Value.String);

        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            (ApplicationUser?)Value.Null);

        _userManagerMockHelper.SetupAsync(
            x => x.VerifyUserTokenAsync(Any.ApplicationUser, Any.String, Any.String, Any.String),
            Value.True);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.InvalidCredentials);
    }

    [Fact]
    public async Task InvalidToken_ShouldReturn_Failure()
    {
        // Arrange
        _jwtProviderMock.Setup(x => x.GetEmailFromToken(Any.String))
            .Returns(Value.String);

        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            Value.ApplicationUser);

        _userManagerMockHelper.SetupAsync(
            x => x.VerifyUserTokenAsync(Any.ApplicationUser, Any.String, Any.String, Any.String),
            Value.False);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.InvalidCredentials);
    }
}
