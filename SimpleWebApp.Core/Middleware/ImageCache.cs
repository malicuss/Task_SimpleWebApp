using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleWebApp.Core.Helpers;
using SimpleWebApp.Core.Models;

namespace SimpleWebApp.Core.Middleware;

public class ImageCache
{
    private readonly ILogger<ImageCache> _logger;
    private readonly RequestDelegate _next;
    private readonly ICacher _cacher;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ImageCache(ILogger<ImageCache> logger,
        RequestDelegate next, 
        ICacher cacher,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _next = next;
        _cacher = cacher;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        
        if (await CheckRequest(context))
            await _next(context);
        EvaluateResponse(context);
    }

    private async Task<bool> CheckRequest(HttpContext context)
    {
        if (context.Request.RouteValues.ContainsKey("controller") &&
            context.Request.RouteValues["controller"].Equals("Categories"))
            ApplyCache(context);
        return true;
    }

    private void ApplyCache(HttpContext context)
    {
        if (context.Request.RouteValues.Contains(new KeyValuePair<string, object?>("action", "Image"))||
           context.Request.RouteValues.Contains(new KeyValuePair<string, object?>("action", "ImageForm")))
        {
            string id = string.Empty;
            if (context.Request.RouteValues.ContainsKey("imageId"))
                id = context.Request.RouteValues["imageId"].ToString();
            else
            {
                id = context.Request.Query["imageId"].ToString();
            }
            var res =_cacher.GetCachedObject(out var obj,id);
            if (res)
            {
                context.Items["imgString"]=id;
                context.Items[$"imgString_{id}"]=obj;
            }
        }
        else
        {
            foreach (var cat in GetCategories())
            {
                if (_cacher.GetCachedObject(out var obj, cat.CategoryId.ToString()) &&
                    context.Request.RouteValues.Contains(new KeyValuePair<string, object?>("action", "List")))
                {
                    var id = cat.CategoryId.ToString();
                    context.Items[$"imgString_{id}"]=obj;
                }
            }
        }
    }

    private void EvaluateResponse(HttpContext context)
    {
        if (context.Request.Method.Equals("POST")) return;
        foreach (var item in context.Response.HttpContext.Items)
        {
            if (item.Key.ToString().Contains("imgString_"))
            {
                _cacher.SaveObjectToCache(item.Value, item.Key.ToString().Trim("imgString_".ToCharArray()));
            }
        }
    }
    
    private List<Category> GetCategories()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetService<IDbContextWrapper>();
        return service.GetCategoriesFromDb().GetAwaiter().GetResult();
    }
}