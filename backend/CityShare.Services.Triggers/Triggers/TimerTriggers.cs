using CityShare.Backend.Application.Core.Dtos.Triggers;
using CityShare.Backend.Application.Emails.Commands;
using CityShare.Backend.Domain.Constants;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CityShare.Services.Triggers.Triggers
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
    }
}
