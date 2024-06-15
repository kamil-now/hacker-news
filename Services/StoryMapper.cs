using HackerNews.Models;

namespace HackerNews.Services;

public static class StoryMapper
{
  public static Story? MapFromHackerNewsResponse(HackerNewsAPIResponseItem? response)
    => response == null 
      ? null 
      : new(
          response.Title, 
          response.Url, 
          response.By, 
          DateTimeOffset.FromUnixTimeSeconds(response.Time), 
          response.Score, 
          response.Descendants);
}