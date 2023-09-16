using CityShare.Backend.Application.Core.Abstractions.Cache;
using CityShare.Backend.Domain.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Infrastructure.Cache;

public class InMemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private CacheSettings _cacheSettings;

    public InMemoryCacheService(IMemoryCache memoryCache, IOptions<CacheSettings> options)
    {
        _cacheSettings = options.Value;
        _memoryCache = memoryCache;
    }

    public bool TryGet<T>(object key, out T? value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }

    public T? Get<T>(object key)
    {
        return _memoryCache.Get<T>(key);
    }

    public void Set<T>(object key, T value, int size = 1)
    {
        var options = new MemoryCacheEntryOptions();

        if (_cacheSettings.AbsoluteExpirationSeconds is not null)
        {
            var seconds = (double)_cacheSettings.AbsoluteExpirationSeconds;
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds);
        }

        if (_cacheSettings.SlidingExpirationSeconds is not null)
        {
            var seconds = (double)_cacheSettings.SlidingExpirationSeconds;
            options.SlidingExpiration = TimeSpan.FromSeconds(seconds);
        }

        options.Size = size;

        _memoryCache.Set<T>(key, value, options);
    }
}
