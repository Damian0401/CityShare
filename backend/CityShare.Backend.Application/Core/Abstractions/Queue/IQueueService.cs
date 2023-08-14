namespace CityShare.Backend.Application.Core.Abstractions.Queue;

public interface IQueueService
{
    Task SendAsync<T>(string queueName, T item, bool encodeToBase64 = true, bool createIfNotExists = true, CancellationToken cancellationToken = default);
}
