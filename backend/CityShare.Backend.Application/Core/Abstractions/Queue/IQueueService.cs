namespace CityShare.Backend.Application.Core.Abstractions.Queue;

public interface IQueueService
{
    Task SendAsync<T>(
        string queueName, 
        T item,
        QueueServiceOptions? options = null,
        CancellationToken cancellationToken = default);
}

public class QueueServiceOptions
{
    public bool? EncodeToBase64 { get; set; }
    public bool? CreateIfNotExists { get; set; }
}