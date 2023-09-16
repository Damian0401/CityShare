namespace CityShare.Backend.Application.Core.Abstractions.Cache;

public interface ICacheService
{
    T? Get<T>(object key);
    bool TryGet<T>(object key, out T? value);
    void Set<T>(object key, T value, int size = 1);
}
