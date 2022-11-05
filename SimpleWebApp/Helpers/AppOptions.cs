namespace SimpleWebApp.Helpers;

public class AppOptions
{
    public static string Options = "Options";

    public int MaxProductsToShow { get; set; }
    public int CacheLifeTime { get; set; }
    public int MaxImagesInCache { get; set; }
    public bool LogActionParameters { get; set; }
}