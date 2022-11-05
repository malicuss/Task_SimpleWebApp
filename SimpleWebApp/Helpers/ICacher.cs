namespace SimpleWebApp.Helpers;

public interface ICacher
{
    bool GetCachedImage(out string value, int id);
    bool SaveImageToCache(string value, int id);
}