﻿using CityShare.Backend.Application.Core.Abstractions.Cache;
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

    public void Set<T>(object key, T value, CacheServiceOptions? options = null)
    {
        var entryOptions = new MemoryCacheEntryOptions();

        SetAbsoluteExpirationTime(options, entryOptions);
        SetSlidingExpirationTime(options, entryOptions);
        entryOptions.Size = options?.Size ?? 1;

        _memoryCache.Set(key, value, entryOptions);
    }

    private void SetSlidingExpirationTime(CacheServiceOptions? options, MemoryCacheEntryOptions entryOptions)
    {
        if (options?.SlidingExpirationSeconds is not null)
        {
            var seconds = options.SlidingExpirationSeconds.Value;
            entryOptions.SlidingExpiration = TimeSpan.FromSeconds(seconds);

            return;
        }

        if (_cacheSettings.SlidingExpirationSeconds is not null)
        {
            var seconds = (double)_cacheSettings.SlidingExpirationSeconds;
            entryOptions.SlidingExpiration = TimeSpan.FromSeconds(seconds);
        }
    }

    private void SetAbsoluteExpirationTime(CacheServiceOptions? options, MemoryCacheEntryOptions entryOptions)
    {
        if (options?.AbsotuleExpirationSeconds is not null)
        {
            var seconds = options.AbsotuleExpirationSeconds.Value;
            entryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds);

            return;
        }

        if (_cacheSettings.AbsoluteExpirationSeconds is not null)
        {
            var seconds = (double)_cacheSettings.AbsoluteExpirationSeconds;
            entryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds);
        }
    }
}
