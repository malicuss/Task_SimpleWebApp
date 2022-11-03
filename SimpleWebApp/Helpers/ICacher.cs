namespace SimpleWebApp.Helpers;

public interface ICacher
{
    bool GetCachedImage(out string value, int id);
    bool SaveImageToFile(string value, int id);
}