public interface ICacheRepository
{
    Task<bool> SetCacheAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null);
    Task<bool> RemoveCacheAsync(string key);
    Task<T?> GetCacheAsync<T>(string key);
    Task<bool> IsCacheExistAsync(string key);
}