﻿using CityShare.Backend.Application.Authentication.Commands.Register;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Models.Authentication.Register;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using CityShare.Backend.Tests.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Authentication;

public class RegisterCommandHandlerTests
{
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly RegisterCommand _registerCommand;
    private readonly RegisterCommandHandler _systemUnderTests;

    public RegisterCommandHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        _jwtProviderMock = new Mock<IJwtProvider>();

        var mapper = MapperHelper.GetMapper();

        var request = new RegisterRequestModel
        {
            Email = Value.String,
            Password = Value.String,
            UserName = Value.String,
        };

        var logger = new Mock<ILogger<RegisterCommandHandler>>().Object;

        _registerCommand = new RegisterCommand(request);

        _systemUnderTests = new RegisterCommandHandler(
            _userManagerMockHelper.GetMockObject(),
            _jwtProviderMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task EmailTaken_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            Value.ApplicationUser);

        // Act
        var result = await _systemUnderTests
            .Handle(_registerCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task UserNotCreated_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            (ApplicationUser?)Value.Null);

        _userManagerMockHelper.SetupAsync(
            x => x.CreateAsync(Any.ApplicationUser, Any.String),
            Value.IdentityResultFailed);

        // Act
        var result = await _systemUnderTests.Handle(_registerCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            (ApplicationUser?)Value.Null);

        _userManagerMockHelper.SetupAsync(
            x => x.CreateAsync(Any.ApplicationUser, Value.String),
            Value.IdentityResultSecceeded);

        // Act
        var result = await _systemUnderTests.Handle(_registerCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
