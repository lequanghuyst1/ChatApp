public class CacheRepository : ICacheRepository
{
       private readonly IDistributedCache _cache;
       public CacheRepository(IDistributedCache cache)
       {
           _cache = cache;
       }
       public async Task<bool> SetCacheAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
       {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = absoluteExpiration
            };
           await _cache.SetAsync(key, value, options);
           return true;
       }

       public async Task<bool> RemoveCacheAsync(string key)
       {
           await _cache.RemoveAsync(key);
           return true;
       }

       public async Task<T?> GetCacheAsync<T>(string key)
       {
           return await _cache.GetAsync<T>(key);
       }
       
       public async Task<bool> IsCacheExistAsync(string key)
       {
           return await _cache.ExistsAsync(key);
       }
}