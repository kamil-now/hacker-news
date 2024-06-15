using HackerNews.Models;

namespace HackerNews.Services;

public class HackerNewsService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IHackerNewsService
{
  readonly string BestStoriesUrl = configuration.GetSection("HackerNewsAPI:BestStoriesUrl")?.Value 
    ?? throw new Exception("Missing configuration for HackerNewsAPI:BestStoriesUrl");
  readonly string StoryDetailUrlTemplate =configuration.GetSection("HackerNewsAPI:StoryDetailTemplateUrl")?.Value 
    ?? throw new Exception("Missing configuration for HackerNewsAPI:StoryDetailTemplateUrl"); 

  public async Task<IEnumerable<Story>?> TryGetBestStoriesAsync(int n)
  {
    var stories = await QueryHackerNewsAPIAsync(n);
    if (stories == null)
    {
      return null;
    }

    return stories;
  }

  private async Task<IEnumerable<Story>?> QueryHackerNewsAPIAsync(int n)
  {
    var httpClient = httpClientFactory.CreateClient();
    var storyIds = await httpClient.GetFromJsonAsync<IEnumerable<int>>(BestStoriesUrl);

    if (storyIds == null)
    {
      return null;
    }

    var tasks = storyIds.Take(n).Select(async id =>
    {
      var storyUrl = string.Format(StoryDetailUrlTemplate, id);
      return await httpClient.GetFromJsonAsync<HackerNewsAPIResponseItem>(storyUrl);
    });

    return [.. (await Task.WhenAll(tasks))
        .Where(story => story != null)
        .OrderByDescending(story => story?.Score)
        .Take(n)
        .Select(StoryMapper.MapFromHackerNewsResponse)];
  }
}