using System.Net;
using System.Text;
using System.Timers;
using Microsoft.Extensions.Options;

namespace SimpleWebApp.Helpers;

public class ImageCacher: ICacher
{
    private static string _fileCacheImageExtension = "imgCache";
    private readonly ILogger<ICacher> _logger;
    private readonly string _pathToCache;
    private static System.Timers.Timer _cacheTimeTicker;
    private readonly int _imageCount;
    
    public ImageCacher(ILogger<ImageCacher> logger,IOptions<AppOptions> options)
    {
        _logger = logger;
        _pathToCache = options.Value.CachePath;
        _imageCount = options.Value.MaxImageInCache;
        SetUpTimer(options.Value.CacheLifeTime);
    }

    public bool GetCachedImage(out string value, int id)
    {
        value = String.Empty;
        var result = true;
        var path = Path.Combine(_pathToCache,$"{id}.{_fileCacheImageExtension}");
        try
        {
            if (File.Exists(path))
            {
                using var fs = File.OpenRead(path);
                value = new UTF8Encoding(true).GetString(File.ReadAllBytes(path));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to save file cache");
            result = false;
        }

        return result;
    }

    public bool SaveImageToFile(string value, int id)
    {
        var result = true;
        var path = Path.Combine(_pathToCache,$"{id}.{_fileCacheImageExtension}");
        try
        {
            using var fs = File.Open(path,FileMode.OpenOrCreate);
            var info = new UTF8Encoding(true).GetBytes(value);
            fs.WriteAsync(info, 0, info.Length).GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to save file cache");
            result = false;
        }

        return result;
    }

    private static void ClearCache(string pathToCache)
    {
        var lok = new object();
        lock (lok)
        {
            var fs = Directory.EnumerateFiles(pathToCache);
            if (fs.Any())
            {
                foreach (var file in fs)
                {
                    File.Delete(file);
                }
            }
        }
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e) => ClearCache(_pathToCache);

    private void SetUpTimer(int timerLifeTime)
    {
        _cacheTimeTicker = new System.Timers.Timer(timerLifeTime);
        _cacheTimeTicker.AutoReset = false;
        _cacheTimeTicker.Enabled = false;
        _cacheTimeTicker.Elapsed += OnTimedEvent;
    }
}