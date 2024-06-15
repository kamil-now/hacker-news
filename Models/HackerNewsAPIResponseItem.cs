namespace HackerNews.Models;

public record HackerNewsAPIResponseItem(string By, int Descendants, int Id, int Score, int Time, string Title, string Type, string Url);
