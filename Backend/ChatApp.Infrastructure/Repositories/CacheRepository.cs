using Microsoft.Extensions.Caching.Memory;

public class CacheRepository : ICacheRepository
{
    private readonly IMemoryCache _cache;

    public CacheRepository(IMemoryCache cache)
    {
        _cache = cache;
    }

    public bool SetCache<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration
        };
        _cache.Set(key, value, options);
        return true; // Trả về true nếu set thành công (giả định)
    }

    public bool RemoveCache(string key)
    {
        _cache.Remove(key);
        return true; // Trả về true nếu remove thành công (giả định)
    }

    public T? GetCache<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    public bool IsCacheExist(string key)
    {
        return _cache.TryGetValue(key, out _); // Kiểm tra xem key có tồn tại không
    }
}