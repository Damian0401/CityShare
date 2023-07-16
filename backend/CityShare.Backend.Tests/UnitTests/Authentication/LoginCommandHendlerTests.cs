using AutoMapper;
using CityShare.Backend.Application.Authentication.Commands.Login;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Contracts.Authentication.Login;
using CityShare.Backend.Application.Core.Mappers;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Tests.Common;
using CityShare.Backend.Tests.Helpers;
using CityShare.Backend.Tests.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Authentication;

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

        var jwtSettings = Options.Create(new JwtSettings());

        var mapper = new MapperConfiguration(
            config => config.AddProfile<AutoMapperProfile>())
            .CreateMapper();

        var request = new LoginRequest
        {
            Email = Value.String,
            Password = Value.String,
        };

        _loginCommand = new LoginCommand(request);

        _systemUnderTests = new LoginCommandHandler(
            _userManagerMockHelper.GetMockObject(),
            jwtSettings,
            _jwtProviderMock.Object,
            mapper);
    }

    [Fact]
    public async Task UserNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Value.String),
            (ApplicationUser?)Value.Null);

        // Act
        var result = await _systemUnderTests
            .Handle(_loginCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task IncorrectPassword_ShouldReturn_Failure()
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
