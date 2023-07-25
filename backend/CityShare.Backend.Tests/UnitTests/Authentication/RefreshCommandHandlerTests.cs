using AutoMapper;
using Castle.Core.Logging;
using CityShare.Backend.Application.Authentication.Commands.Refresh;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Models.Authentication.Refresh;
using CityShare.Backend.Application.Core.Mappers;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using CityShare.Backend.Tests.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Authentication;

public class RefreshCommandHandlerTests
{
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly RefreshCommand _refreshCommand;
    private readonly RefreshCommandHandler _systemUnderTests;

    public RefreshCommandHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        _jwtProviderMock = new Mock<IJwtProvider>();

        var jwtSettings = Options.Create(new JwtSettings());

        var mapper = new MapperConfiguration(
            config => config.AddProfile<AutoMapperProfile>())
            .CreateMapper();

        var request = new RefreshRequestModel
        {
            AccessToken = Value.String
        };

        var logger = new Mock<ILogger<RefreshCommandHandler>>().Object;

        _refreshCommand = new RefreshCommand(request, Value.String);

        _systemUnderTests = new RefreshCommandHandler(
            _userManagerMockHelper.GetMockObject(),
            _jwtProviderMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task EmailNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _jwtProviderMock.Setup(x => x.GetEmailFromToken(Any.String))
            .Returns((string?)Value.Null);

        // Act
        var result = await _systemUnderTests.Handle(_refreshCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
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

        // Act
        var result = await _systemUnderTests.Handle(_refreshCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
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
        var result = await _systemUnderTests.Handle(_refreshCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Failure()
    {
        // Arrange
        _jwtProviderMock.Setup(x => x.GetEmailFromToken(Any.String))
            .Returns(Value.String);

        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            Value.ApplicationUser);

        _userManagerMockHelper.SetupAsync(
            x => x.VerifyUserTokenAsync(Any.ApplicationUser, Any.String, Any.String, Value.String),
            Value.True);

        // Act
        var result = await _systemUnderTests.Handle(_refreshCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
