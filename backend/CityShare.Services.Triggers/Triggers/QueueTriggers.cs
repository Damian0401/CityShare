using CityShare.Backend.Application.Emails.Commands;
using CityShare.Backend.Application.Images.Commands;
using CityShare.Backend.Domain.Constants;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CityShare.Services.Triggers.Triggers;

public class QueueTriggers
{
    private readonly ILogger<QueueTriggers> _logger;
    private readonly IMediator _mediator;

    public QueueTriggers(ILogger<QueueTriggers> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Function(nameof(SendEmail))]
    public async Task SendEmail(
        [QueueTrigger(QueueNames.EmailsToSend, Connection = ConnectionStrings.StorageAccount)] Guid emailId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing {@Name} trigger for id {@Id}", nameof(SendEmail), emailId);

        var response = await _mediator.Send(new SendEmailCommand(emailId), cancellationToken);

        if (response.IsFailure)
        {
            _logger.LogError("Failed to execute {@Name} trigger with errors {@Errors}", nameof(SendEmail), response.Errors);
            return;
        }

        _logger.LogInformation("Executed {@Name} trigger successfully", nameof(SendEmail));
    }

    [Function(nameof(BlurImage))]
    public async Task BlurImage(
        [QueueTrigger(QueueNames.ImagesToBlur, Connection = ConnectionStrings.StorageAccount)] Guid imageId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing {@Name} trigger for id {@Id}", nameof(BlurImage), imageId);

        var response = await _mediator.Send(new BlurFacesCommand(imageId), cancellationToken);

        if (response.IsFailure)
        {
            _logger.LogError("Failed to execute {@Name} trigger with errors {@Errors}", nameof(BlurImage), response.Errors);
            return;
        }

        _logger.LogInformation("Executed {@Name} trigger successfully", nameof(BlurImage));
    }
}
