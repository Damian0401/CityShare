using CityShare.Backend.Application.Authentication.Commands;
using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Queue;
using CityShare.Backend.Application.Core.Dtos.Authentication.Register;
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
    private readonly Register.Command _registerCommand;
    private readonly Register.Handler _systemUnderTests;

    public RegisterCommandHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        _jwtProviderMock = new Mock<IJwtProvider>();

        var mapper = MapperHelper.GetMapper();

        var options = Options.Create(new CommonSettings());

        var queueServiceMock = new Mock<IQueueService>();

        var emailRepositoryMock = new Mock<IEmailRepository>();

        var logger = new Mock<ILogger<Register.Handler>>().Object;

        var request = new RegisterRequestDto(Value.String, Value.String, Value.String);

        _registerCommand = new Register.Command(request);

        _systemUnderTests = new Register.Handler(
            _userManagerMockHelper.GetMockObject(),
            _jwtProviderMock.Object,
            mapper,
            options,
            queueServiceMock.Object,
            emailRepositoryMock.Object,
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
            x => x.CreateAsync(Any.ApplicationUser, Any.String),
            Value.IdentityResultSecceeded);

        // Act
        var result = await _systemUnderTests.Handle(_registerCommand, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
