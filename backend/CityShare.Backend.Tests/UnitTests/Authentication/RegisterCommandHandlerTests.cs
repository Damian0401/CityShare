using AutoMapper;
using CityShare.Backend.Application.Authentication.Commands.Refresh;
using CityShare.Backend.Application.Authentication.Commands.Register;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Contracts.Authentication.Register;
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

        var jwtSettings = Options.Create(new JwtSettings());

        var mapper = new MapperConfiguration(
            config => config.AddProfile<AutoMapperProfile>())
            .CreateMapper();

        var request = new RegisterRequest
        {
            Email = Value.String,
            Password = Value.String,
            UserName = Value.String,
        };

        var logger = new Mock<ILogger<RegisterCommandHandler>>().Object;

        _registerCommand = new RegisterCommand(request);

        _systemUnderTests = new RegisterCommandHandler(
            _userManagerMockHelper.GetMockObject(),
            jwtSettings,
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
