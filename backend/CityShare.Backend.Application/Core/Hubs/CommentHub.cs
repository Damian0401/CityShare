using CityShare.Backend.Application.Core.Abstractions.Comments;
using CityShare.Backend.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Core.Hubs;

[Authorize]
public class CommentHub : Hub<ICommentClient>
{
    private readonly ILogger<CommentHub> _logger;

    public CommentHub(ILogger<CommentHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var context = Context.GetHttpContext();

        ArgumentNullException.ThrowIfNull(context);

        if (!Guid.TryParse(context.Request.Query[QueryParameters.EventId], out var eventId))
        {
            throw new InvalidCastException();
        };

        _logger.LogInformation("Connecting to {@Id} group", eventId);
        await Groups.AddToGroupAsync(Context.ConnectionId, eventId.ToString());

        await base.OnConnectedAsync();
    }
}
