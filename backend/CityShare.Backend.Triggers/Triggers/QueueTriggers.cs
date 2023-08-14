using CityShare.Backend.Application.Emails.Commands.SendNewEmail;
using CityShare.Backend.Domain.Constants;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Triggers.Triggers;

public class QueueTriggers
{
    private readonly ILogger<QueueTriggers> _logger;
    private readonly IMediator _mediator;

    public QueueTriggers(ILogger<QueueTriggers> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Function(nameof(SendNewEmail))]
    public async Task SendNewEmail(
        [QueueTrigger(QueueNames.EmailsToSend, Connection = ConnectionStrings.StorageAccount)] Guid emailId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing {@Name} trigger for id {@Id}", nameof(SendNewEmail), emailId);

        var result = await _mediator.Send(new SendNewEmailCommand(emailId), cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to execute {@Name} trigger with errors {@Errors}", nameof(SendNewEmail), result.Errors);
            return;
        }

        _logger.LogInformation("Execution {@Name} trigger completed", nameof(SendNewEmail));
    }
}
