﻿using SimpleWebApp.Helpers;

namespace SimpleWebApp.Middleware;

public class ImageCache
{
    // Check Content-Type for every response, and, if any image of a valid format returned:
    //      Keep the image on the disk (as file)
    //      If subsequent requests access the same image, return it from the cache directory
    // Support the following options:
    //      The path for the cache directory
    //      Max count of cached images 
    //      Cache expiration time (if no requests during this time, cache cleaned) 
    
    private readonly RequestDelegate _next;
    private readonly ICacher _cacher;

    public ImageCache(RequestDelegate next, ICacher cacher )
    {
        _next = next;
        _cacher = cacher;
    }

    public async Task Invoke(HttpContext context)
    {
        var req = context.Request;
        
        var res = context.Response;
        var z = res.ContentType;
        await _next(context);
        var req1 = context.Request;
        var res1 = context.Response;
    }
}