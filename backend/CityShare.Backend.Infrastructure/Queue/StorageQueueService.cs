using Azure.Storage.Queues;
using CityShare.Backend.Application.Core.Abstractions.Queue;
using System.Text.Json;

namespace CityShare.Backend.Infrastructure.Queue;

public class StorageQueueService : IQueueService
{
    private readonly QueueServiceClient _queueServiceClient;

    public StorageQueueService(QueueServiceClient queueServiceClient)
    {
        _queueServiceClient = queueServiceClient;
    }

    public async Task SendAsync<T>(string queueName, T item, bool createIfNotExists = true)
    {
        var queueClient = _queueServiceClient.GetQueueClient(queueName);

        if (createIfNotExists)
        {
            await queueClient.CreateIfNotExistsAsync();
        }

        var message = JsonSerializer.Serialize(item);

        await queueClient.SendMessageAsync(message);
    }
}
