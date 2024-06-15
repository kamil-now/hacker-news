using HackerNews.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddHttpClient();

builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IHackerNewsService, HackerNewsService>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hacker News V1");
});
app.UseHttpsRedirection();

app.MapGet("/api/best-stories/{n:int}", async (int n, IHackerNewsService service) =>
{
    if (n <= 0 || n > 500)
        return Results.BadRequest("Invalid value for 'n'. Must be a value between 1 and 500.");
    
    var stories = await service.TryGetBestStoriesAsync(n);
    
    return stories == null ? Results.NoContent() : Results.Ok(stories);
}).WithName("GetBestStories").WithOpenApi();


app.Run();
