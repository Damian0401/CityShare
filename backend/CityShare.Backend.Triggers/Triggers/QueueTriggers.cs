using CityShare.Backend.Domain.Constants;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Triggers.Triggers
{
    public class QueueTriggers
    {
        private readonly ILogger _logger;

        public QueueTriggers(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<QueueTriggers>();
        }

        [Function(nameof(ProcessEmails))]
        public void ProcessEmails(
            [QueueTrigger(QueueNames.EmailsToSend, Connection = ConnectionStrings.StorageAccount)] Guid emailId)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {emailId}");
        }
    }
}
