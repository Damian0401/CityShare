using CityShare.Backend.Application.Auth.Commands;
using CityShare.Backend.Application.Core.Abstractions.Auth;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Auth;

public class LoginCommandHandlerTests
{
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly LoginCommand _loginCommand;
    private readonly LoginCommandHandler _systemUnderTests;

    public LoginCommandHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        _jwtProviderMock = new Mock<IJwtProvider>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<LoginCommandHandler>>().Object;

        var request = new LoginRequestDto(Value.String, Value.String);

        _loginCommand = new LoginCommand(request);

        _systemUnderTests = new LoginCommandHandler(
            _userManagerMockHelper.Object,
            _jwtProviderMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task UserNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            (ApplicationUser?)Value.Null);

        _userManagerMockHelper.SetupAsync(
            x => x.CheckPasswordAsync(Any.ApplicationUser, Any.String),
            Value.True);

        // Act
        var result = await _systemUnderTests
            .Handle(_loginCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task InvalidPassword_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            Value.ApplicationUser);

        _userManagerMockHelper.SetupAsync(
            x => x.CheckPasswordAsync(Any.ApplicationUser, Any.String),
            Value.False);

        // Act
        var result = await _systemUnderTests.Handle(_loginCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            Value.ApplicationUser);

        _userManagerMockHelper.SetupAsync(
            x => x.CheckPasswordAsync(Any.ApplicationUser, Any.String),
            Value.True);

        // Act
        var result = await _systemUnderTests.Handle(_loginCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
