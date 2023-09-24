using Azure.Storage.Queues;
using CityShare.Backend.Application.Core.Abstractions.Queues;
using System.Text;
using System.Text.Json;

namespace CityShare.Backend.Infrastructure.Queues;

public class StorageQueueService : IQueueService
{
    private readonly QueueServiceClient _queueServiceClient;

    public StorageQueueService(QueueServiceClient queueServiceClient)
    {
        _queueServiceClient = queueServiceClient;
    }

    public async Task SendAsync<T>(string queueName, 
        T item, 
        QueueServiceOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        var queueClient = _queueServiceClient.GetQueueClient(queueName);

        var createIfNotExists = options?.CreateIfNotExists ?? false;
        if (createIfNotExists)
        {
            await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        }

        var message = JsonSerializer.Serialize(item);

        var encodeToBase64 = options?.EncodeToBase64 ?? false;
        if (encodeToBase64)
        {
            message = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }

        await queueClient.SendMessageAsync(message, cancellationToken);
    }
}
