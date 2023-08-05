namespace CityShare.Backend.Application.Core.Abstractions.Queue;

public interface IQueueService
{
    Task SendAsync<T>(string queueName, T item, bool createIfNotExists = true);
}
