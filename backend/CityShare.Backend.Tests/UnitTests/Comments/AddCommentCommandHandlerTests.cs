using Castle.Core.Logging;
using CityShare.Backend.Application.Comments.Commands;
using CityShare.Backend.Application.Core.Abstractions.Comments;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Application.Core.Hubs;
using CityShare.Backend.Tests.Other.Common;
using CityShare.Backend.Tests.Other.Helpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;

namespace CityShare.Backend.Tests.UnitTests.Comments;

public class AddCommentCommandHandlerTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly AddCommentCommand _command;
    private readonly AddCommentCommandHandler _systemUnderTests;
    private readonly Mock<IHubContext<CommentHub, ICommentClient>> _contextMock;

    public AddCommentCommandHandlerTests()
    {
        _commentRepositoryMock = new Mock<ICommentRepository>();

        var clock = new Mock<IClock>();

        _contextMock = new Mock<IHubContext<CommentHub, ICommentClient>>();

        var mapper = MapperHelper.GetMapper();

        var logger = new Mock<ILogger<AddCommentCommandHandler>>().Object;

        _command = new AddCommentCommand(Value.Guid, Value.String, Value.String, Value.CreateCommentDto);

        _systemUnderTests = new AddCommentCommandHandler(
            _commentRepositoryMock.Object,
            clock.Object,
            _contextMock.Object,
            mapper,
            logger);
    }

    [Fact]
    public async Task CorrectRequest_ShouldCall_CommentContext()
    {
        // Arrange
        _contextMock.Setup(x => x.Clients.Group(Any.String).AddCommentAsync(Any.CommentDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _systemUnderTests.Handle(_command, Value.CancelationToken);

        // Assert
        _contextMock.Verify(x => x.Clients.Group(Any.String), Times.Once());
    }
}
