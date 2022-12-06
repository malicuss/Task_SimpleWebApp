using Microsoft.Extensions.Configuration;

namespace SimpleWebApp.Core.Helpers;

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