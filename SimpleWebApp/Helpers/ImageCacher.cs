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
            calculateCacheSize: false,
            cleanInterval: new TimeSpan(0, 0, options.Value.CacheLifeTime));
    }

    public bool GetCachedImage(out string value, int id)
    {
        value = string.Empty;
        var res = true;

        value = _fileCache.GetCacheItem(id.ToString()).ToString();

        return res;
    }

    public bool SaveImageToCache(string value, int key)
    {
        bool result;
        if (_fileCache.GetCount() > _imageMaxCount)
        {
            _logger.LogInformation("Image limit in cache reached.");
            return false;
        }
        try
        {
            result = _fileCache.Add(key.ToString(), value, new CacheItemPolicy());
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error happens while saving to image'{key}'cache");
            result = false;
        }
        return result;
    }
}