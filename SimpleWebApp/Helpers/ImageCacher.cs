using System.Text;
using Microsoft.Extensions.Options;

namespace SimpleWebApp.Helpers;

public class ImageCacher: ICacher
{
    private readonly ILogger<ICacher> _logger;
    private readonly string _pathToCache;
    
    
    public ImageCacher(ILogger<ImageCacher> logger,IOptions<AppOptions> options)
    {
        _logger = logger;
        _pathToCache = options.Value.CachePath;
    }

    public bool GetCachedImage(out string value, int id)
    {
        value = String.Empty;
        var result = true;
        var path = Path.Combine(_pathToCache,$"{id}.cache");
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
        var path = Path.Combine(_pathToCache,$"{id}.cache");
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
}