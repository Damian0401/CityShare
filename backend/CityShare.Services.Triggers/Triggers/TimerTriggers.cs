using MediatR;
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
