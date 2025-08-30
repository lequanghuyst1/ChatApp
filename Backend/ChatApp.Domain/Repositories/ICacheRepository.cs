public interface ICacheRepository
{
    bool SetCache<T>(string key, T value, TimeSpan? absoluteExpiration = null);
    bool RemoveCache(string key);
    T? GetCache<T>(string key);
    bool IsCacheExist(string key);
}