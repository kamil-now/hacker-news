
using System.Text.Json;
using HackerNews.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace HackerNews.Services;

public class CacheService(IDistributedCache cache) : ICacheService
{
  private static string GetStoriesCacheKey(int n) => $"stories_{n}";
  public async Task<IEnumerable<Story>?> TryGetStoriesFromCacheAsync(int n) 
    => await TryGetFromCacheAsync<IEnumerable<Story>>(GetStoriesCacheKey(n));

  public async Task SaveStoriesInCacheAsync(IEnumerable<Story?> stories)
    => await SaveInCacheAsync(GetStoriesCacheKey(stories.Count()), stories);

  public async Task<T?> TryGetFromCacheAsync<T>(string cacheKey) where T : class
  {
    var cachedData = await cache.GetStringAsync(cacheKey);
    if (cachedData != null)
    {
      return JsonSerializer.Deserialize<T>(cachedData);
    }
    return null;
  }

  public async Task SaveInCacheAsync<T>(string cacheKey, T data)
  {
    var serializedData = JsonSerializer.Serialize(data);
    await cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
    {
      AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
    });
  }
}