using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        var req = context.Request;
        if (req.Path.Value.Contains("Image"))
        {
            var id = int.Parse(req.Path.Value.Split('/').Last());
            if (_cacher.GetCachedImage(out var base64Img, id))
                await Test(context, base64Img);
        }

        await _next(context);
        var image = context.Response.Headers.FirstOrDefault(
            x => x.Key.Equals("ImageInData")).Value;
        if (image.Count > 0)
        {
            var k = context.Items["imgString"];
                _cacher.SaveImageToCache(image, 12);
        }
    }

    private async Task Test(HttpContext context, string image)
    {
        var viewResult = new ViewResult()
        {
            ViewName = "Image"//path to view e.g. ~/Views/Tests/Index.cshtml,
        };
        var viewDataDictionary = new ViewDataDictionary(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary());
        viewDataDictionary["imgString"] = image;
        viewResult.ViewData = viewDataDictionary;

            
        var executor = context.RequestServices
            .GetRequiredService<IActionResultExecutor<ViewResult>>();
        var routeData = context.GetRouteData() ?? new RouteData();
        var actionContext = new ActionContext(context, routeData,
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        await executor.ExecuteAsync(actionContext, viewResult);   
    }
}