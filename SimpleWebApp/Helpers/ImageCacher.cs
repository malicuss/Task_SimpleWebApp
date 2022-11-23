using System.Runtime.Caching;
using Microsoft.Extensions.Options;

namespace SimpleWebApp.Helpers;

public class ImageCacher: ICacher
{
    private readonly ILogger<ICacher> _logger;
    private readonly int _imageMaxCount;
    private readonly FileCache _fileCache;
    
    public ImageCacher(ILogger<ImageCacher> logger,IOptions<AppOptions> options)
    {
        _logger = logger;
        _imageMaxCount = options.Value.MaxImagesInCache;
        _fileCache = new FileCache(
            Path.GetFullPath(options.Value.CacheRootPath),
            calculateCacheSize: false,
            cleanInterval: new TimeSpan(0, 0, options.Value.CacheLifeTime));
    }

    public bool GetCachedObject(out object value, string id)
    {
        value = new object();
        var res = false;
        try
        {
            var cv = _fileCache.GetCacheItem(id.ToString());
            if (cv.Value != null)
            {
                value = cv.Value;
                res = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured during cache retrieving");
        }

        return res;
    }

    public bool SaveObjectToCache(object value, string key)
    {
        bool result;
        if (_fileCache.GetCount() > _imageMaxCount)
        {
            _logger.LogInformation("Image limit in cache reached.");
            return false;
        }
        try
        {
            result = _fileCache.Add(key, value, new CacheItemPolicy());
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error happens while saving to image'{key}'cache");
            result = false;
        }
        return result;
    }
}