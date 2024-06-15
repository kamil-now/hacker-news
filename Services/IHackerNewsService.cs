using HackerNews.Models;

namespace HackerNews.Services;

public interface IHackerNewsService
{
  Task<IEnumerable<Story>?> TryGetBestStoriesAsync(int n);
}