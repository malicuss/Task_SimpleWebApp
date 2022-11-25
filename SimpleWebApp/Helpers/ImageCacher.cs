using System.Runtime.Caching;
using System.Timers;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;

namespace SimpleWebApp.Helpers;

public class ImageCacher: ICacher
{
    private readonly ILogger<ICacher> _logger;
    private readonly int _imageMaxCount;
    private readonly FileCache _fileCache;
    private readonly Timer _alarm;
    private bool _alarmState;
    
    public ImageCacher(ILogger<ImageCacher> logger,IOptions<AppOptions> options)
    {
        _logger = logger;
        _imageMaxCount = options.Value.MaxImagesInCache;
        _fileCache = new FileCache(
            Path.GetFullPath(options.Value.CacheRootPath),
            calculateCacheSize: false);
        _alarm = new Timer();
        _alarm.Interval = options.Value.CacheLifeTime * 1000;
        _alarm.Enabled = true;
        _alarm.Elapsed += CleanCache;
    }

    private void CleanCache(Object source, ElapsedEventArgs e)
    {
        _fileCache.Flush();
        _logger.LogTrace("Cache cleaned");
        _alarmState = false;
    }

    private void StartAlarm()
    {
        _alarm.Start();
        _alarmState = true;
        _logger.LogTrace("Cache countdown started.");
    }
    
    private void RestartAlarm()
    {
        if (_alarmState != false)
            _alarm.Stop();
        _alarm.Start();
        _alarmState = true;
        _logger.LogTrace("Cache countdown restarted.");
    }

    public bool GetCachedObject(out object value, string id)
    {
        RestartAlarm();
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

        _logger.LogTrace($"returning {id} from cache");
        return res;
    }

    public bool SaveObjectToCache(object value, string key)
    {
        if (_fileCache.Contains(key))
            return true;
        _alarm.Start();
        _logger.LogTrace("Cache countdown restarted.");
        bool result;
        if (_fileCache.GetCount() > _imageMaxCount)
        {
            _logger.LogInformation("Image limit in cache reached.");
            return false;
        }
        try
        {
            result = _fileCache.Add(key, value, new CacheItemPolicy());
            _logger.LogTrace($"saved {key} to cache");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error happens while saving to image'{key}'cache");
            result = false;
        }
        return result;
    }
}