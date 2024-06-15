using HackerNews.Models;

namespace HackerNews.Services;

public interface ICacheService 
{
  Task<IEnumerable<Story>?> TryGetStoriesFromCacheAsync(int n);
  Task SaveStoriesInCacheAsync(IEnumerable<Story> stories);
  Task<T?> TryGetFromCacheAsync<T>(string cacheKey) where T : class;
  Task SaveInCacheAsync<T>(string cacheKey, T data); 
}