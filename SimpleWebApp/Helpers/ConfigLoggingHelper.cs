namespace SimpleWebApp.Helpers;

public static class ConfigLoggingHelper
{
    public static string GetConfigString(IConfiguration configuration)
    {
        return configuration
            .AsEnumerable()
            .Aggregate(
                string.Empty, 
                (current, item) 
                    => current + $"{item.Key}:{item.Value}\n");
    }
}