using CityShare.Backend.Application.Core.Abstractions.Categories;
using CityShare.Backend.Application.Core.Abstractions.Cities;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Application.Core.Dtos.Events;
using CityShare.Backend.Application.Events.Commands;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using CityShare.Backend.Tests.Other.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Events;

public class CreateEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly IMockHelper<UserManager<ApplicationUser>> _userManagerMockHelper;
    private readonly CreateEventDto _request;
    private readonly CreateEventCommand _command;
    private readonly CreateEventCommandHandler _systemUnderTests;

    public CreateEventCommandHandlerTests()
    {
        _eventRepositoryMock = new Mock<IEventRepository>();

        _cityRepositoryMock = new Mock<ICityRepository>();

        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        var clock = new Mock<IClock>().Object;

        _userManagerMockHelper = new UserManagerMockHelper<ApplicationUser>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<CreateEventCommandHandler>>().Object;

        _request = new CreateEventDto();

        _command = new CreateEventCommand(
            _request, 
            Value.String);

        _systemUnderTests = new CreateEventCommandHandler(
            _eventRepositoryMock.Object,
            _cityRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _userManagerMockHelper.Object,
            clock,
            mapper,
            logger);
    }

    [Fact]
    public async Task CityNotFound_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _cityRepositoryMock.Setup(x => x.ExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.False);

        var categoryIds = new List<int> { Value.Int };
        _request.CategoryIds = categoryIds;
        _categoryRepositoryMock.Setup(x => x.GetAllIdsAsync(Any.CancellationToken))
            .ReturnsAsync(categoryIds);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task EmailNotConfirmed_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.False;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _cityRepositoryMock.Setup(x => x.ExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        var categoryIds = new List<int> { Value.Int };
        _request.CategoryIds = categoryIds;
        _categoryRepositoryMock.Setup(x => x.GetAllIdsAsync(Any.CancellationToken))
            .ReturnsAsync(categoryIds);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task UserNotFound_ShouldReturn_Failure()
    {
        // Arrange
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            (ApplicationUser?)Value.Null);

        _cityRepositoryMock.Setup(x => x.ExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        var categoryIds = new List<int> { Value.Int };
        _request.CategoryIds = categoryIds;
        _categoryRepositoryMock.Setup(x => x.GetAllIdsAsync(Any.CancellationToken))
            .ReturnsAsync(categoryIds);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CategoryNotFound_ShouldReturn_Failure()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _cityRepositoryMock.Setup(x => x.ExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        var categoryId = Value.Int;
        _request.CategoryIds = new List<int> { categoryId };
        _categoryRepositoryMock.Setup(x => x.GetAllIdsAsync(Any.CancellationToken))
            .ReturnsAsync(new List<int> { categoryId + 1 });

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task CorrectRequest_ShouldReturn_Success()
    {
        // Arrange
        var user = Value.ApplicationUser;
        user.EmailConfirmed = Value.True;
        _userManagerMockHelper.SetupAsync(
            x => x.FindByIdAsync(Any.String),
            user);

        _cityRepositoryMock.Setup(x => x.ExistsAsync(Any.Int, Any.CancellationToken))
            .ReturnsAsync(Value.True);

        var categoryIds = new List<int> { Value.Int };
        _request.CategoryIds = categoryIds;
        _categoryRepositoryMock.Setup(x => x.GetAllIdsAsync(Any.CancellationToken))
            .ReturnsAsync(categoryIds);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
