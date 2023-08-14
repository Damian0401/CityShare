using Azure.Storage.Queues;
using CityShare.Backend.Application.Core.Abstractions.Queue;
using System.Text;
using System.Text.Json;

namespace CityShare.Backend.Infrastructure.Queue;

public class StorageQueueService : IQueueService
{
    private readonly QueueServiceClient _queueServiceClient;

    public StorageQueueService(QueueServiceClient queueServiceClient)
    {
        _queueServiceClient = queueServiceClient;
    }

    public async Task SendAsync<T>(string queueName, T item, bool encodeToBase64 = true, bool createIfNotExists = true, CancellationToken cancellationToken = default)
    {
        var queueClient = _queueServiceClient.GetQueueClient(queueName);

        if (createIfNotExists)
        {
            await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        }

        var message = JsonSerializer.Serialize(item);

        if (encodeToBase64)
        {
            message = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }

        await queueClient.SendMessageAsync(message, cancellationToken);
    }
}
