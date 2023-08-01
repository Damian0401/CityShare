namespace CityShare.Backend.Application.Core.Abstractions.Cache;

public interface ICacheService<T>
{
    T? Get(string key);
    bool TryGet(string key, out T? value);
    void Set(string key, T value);
}
