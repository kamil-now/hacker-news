namespace HackerNews.Models;

public record Story(string Title, string Uri, string PostedBy, DateTimeOffset Time, int Score, int CommentCount);
