namespace CityShare.Backend.Application.Core.Abstractions.Cache;

public interface ICacheService
{
    T? Get<T>(string key);
    bool TryGet<T>(string key, out T? value);
    void Set<T>(string key, T value, int size = 1);
}
