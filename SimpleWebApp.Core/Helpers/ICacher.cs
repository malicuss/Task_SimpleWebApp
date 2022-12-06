namespace SimpleWebApp.Core.Helpers;

public interface ICacher
{
    bool GetCachedObject(out object value, string id);
    bool SaveObjectToCache(object value, string id);
}