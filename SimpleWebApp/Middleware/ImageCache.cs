using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SimpleWebApp.Helpers;
using SimpleWebApp.Models;

namespace SimpleWebApp.Middleware;

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
        if (context.Request.Method.Equals("POST")) return true;
            
        if (EvaluateViewPath(context, out var path) &&
            EvaluateCache(context))
        {
            await RunView(context, path);
            return false; // we are good no need to proceed with next()
        }
        return true;
    }
    
    private bool EvaluateViewPath(HttpContext context,out string path)
    {
        path = String.Empty;
        if (!context.Request.RouteValues["controller"].Equals("Categories")) return false;
        path = $"~/Views/{context.Request.RouteValues["controller"]}/{context.Request.RouteValues["action"]}.cshtml";
        return true;
    }

    private bool EvaluateCache(HttpContext context)
    {
        var res = true;
        if (context.Request.RouteValues.Contains(new KeyValuePair<string, object?>("action", "Image")))
        {
            res =_cacher.GetCachedObject(out var obj,context.Request.RouteValues["imageId"].ToString());
        }
        else
        {
            foreach (var cat in GetCategories())
            {
                if (!_cacher.GetCachedObject(out var obj, cat.CategoryId.ToString()))
                {
                    res = false;
                    break;
                }
            }
        }
        return res;
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
    
    private async Task RunView(HttpContext context, string viewToRun)
    {
        var viewDataDictionary = new ViewDataDictionary(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary());
        var executor = context.RequestServices
            .GetRequiredService<IActionResultExecutor<ViewResult>>();
        var routeData = context.GetRouteData() ?? new RouteData();
        var actionContext = new ActionContext(context, routeData,
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        var viewResult = new ViewResult { ViewName = viewToRun };
        if (context.Request.RouteValues["action"].Equals("List"))
        {
            viewDataDictionary.Model = GetCategories();
            FillFromCache(GetCategories().ConvertAll(x=>x.CategoryId.ToString()), 
                ref actionContext);
        }
        else
        {
            viewDataDictionary.Model = context.Request.RouteValues["imageId"];
            FillFromCache(new List<string>(){context.Request.RouteValues["imageId"].ToString()},
                ref actionContext);
        }

        viewResult.ViewData = viewDataDictionary;
        await executor.ExecuteAsync(actionContext, viewResult);   
    }

    private void FillFromCache(List<string> ids,ref ActionContext context)
    {
        foreach (var id in ids)
        {
            if (_cacher.GetCachedObject(out var value, id))
            {
                context.HttpContext.Items.Add($"imgString_{id}", value);
                if (ids.Count<=1)context.HttpContext.Items.Add($"imgString", id);
            }
        }
    }
    
    private List<Category> GetCategories()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetService<IDbContextWrapper>();
        return service.GetCategoriesFromDb();
    }
}