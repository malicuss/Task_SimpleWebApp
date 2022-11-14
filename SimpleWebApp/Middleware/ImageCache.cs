using SimpleWebApp.Helpers;

namespace SimpleWebApp.Middleware;

public class ImageCache
{
    private readonly ILogger<ImageCache> _logger;
    private readonly RequestDelegate _next;
    private readonly ICacher _cacher;

    public ImageCache(ILogger<ImageCache> logger,RequestDelegate next, ICacher cacher )
    {
        _logger = logger;
        _next = next;
        _cacher = cacher;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);
    }
}