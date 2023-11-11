using CityShare.Backend.Application.Auth.Commands;
using CityShare.Backend.Application.Core.Abstractions.Auth;
using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Queues;
using CityShare.Backend.Application.Core.Dtos.Auth;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Auth;

public class RegisterCommandHandlerTests
{
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly RegisterCommand _command;
    private readonly RegisterCommandHandler _systemUnderTests;

    public RegisterCommandHandlerTests()
    {
        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        _jwtProviderMock = new Mock<IJwtProvider>();

        var mapper = MapperHelper.GetMapper();

        var options = Options.Create(new CommonSettings());

        var queueServiceMock = new Mock<IQueueService>();

        var emailRepositoryMock = new Mock<IEmailRepository>();

        var logger = new Mock<ILogger<RegisterCommandHandler>>().Object;

        var request = new RegisterDto(Value.String, Value.String, Value.String);

        _command = new RegisterCommand(request);

        _systemUnderTests = new RegisterCommandHandler(
            _userManagerMockHelper.Object,
            _jwtProviderMock.Object,
            mapper,
            options,
            queueServiceMock.Object,
            emailRepositoryMock.Object,
            logger);
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
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task EmailTaken_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByEmailAsync(Any.String),
            Value.ApplicationUser);

        _userManagerMockHelper.SetupAsync(
            x => x.CreateAsync(Any.ApplicationUser, Any.String),
            Value.IdentityResultSecceeded);

        // Act
        var result = await _systemUnderTests
            .Handle(_command, Value.CancelationToken);

        // Assert
        AssertHelper.FailureWithErrors(result, Errors.EmailTaken(_command.Request.Email));
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
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }
}
