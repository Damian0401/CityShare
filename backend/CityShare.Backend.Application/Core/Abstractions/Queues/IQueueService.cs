namespace CityShare.Backend.Application.Core.Abstractions.Queues;

public interface IQueueService
{
    Task SendAsync<T>(
        string queueName, 
        T item,
        QueueServiceSendOptions? options = null,
        CancellationToken cancellationToken = default);
}

public class QueueServiceSendOptions
{
    public bool? EncodeToBase64 { get; set; }
    public bool? CreateIfNotExists { get; set; }
}