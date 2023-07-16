using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Core.Behaviours;

public class LoggingPipeloneBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingPipeloneBehaviour<TRequest, TResponse>> _logger;

    public LoggingPipeloneBehaviour(ILogger<LoggingPipeloneBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting request {@RequestName}, {@DateTimeUtc}", 
            typeof(TRequest).Name,
            DateTime.UtcNow);

        var result = await next();

        if (result.IsFailure)
        {
            _logger.LogError(
                "Request failure {@RequestName}, {@Errors}, {@DateTimeUtc}",
                typeof(TRequest).Name, 
                result.Errors,
                DateTime.UtcNow);
        }

        _logger.LogInformation(
            "Completed request: {@RequestName}, {@DateTimeUtc}", 
            typeof(TRequest).Name,
            DateTime.UtcNow);

        return result;
    }
}
