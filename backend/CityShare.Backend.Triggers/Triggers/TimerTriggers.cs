using CityShare.Backend.Application.Core.Dtos.Triggers;
using CityShare.Backend.Application.Emails.Commands.SendPendingEmails;
using CityShare.Backend.Domain.Constants;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Triggers.Triggers
{
    public class TimerTriggers
    {
        private readonly ILogger<TimerTriggers> _logger;
        private readonly IMediator _mediator;

        public TimerTriggers(ILogger<TimerTriggers> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Function(nameof(SendPendingEmails))]
        public async Task SendPendingEmails([TimerTrigger(Schedules.OnceEveryHour)] TimerTriggerDto timerTriggerDto)
        {
            _logger.LogInformation("Executing {@Name} trigger at {@Date}", nameof(SendPendingEmails), DateTime.UtcNow);

            var response = await _mediator.Send(new SendPendingEmailsCommand());

            if (response.IsFailure)
            {
                _logger.LogError("Failed to execute {@Name} trigger with errors {@Errors}", nameof(SendPendingEmails), response.Errors);
                return;
            }

            _logger.LogInformation("Execution {@Name} trigger completed with result {@Result}", nameof(SendPendingEmails), response.Value);
        }
    }
}
