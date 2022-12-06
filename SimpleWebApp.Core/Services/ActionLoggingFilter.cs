using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using SimpleWebApp.Core.Helpers;

namespace SimpleWebApp.Core.Services;

public class ActionLoggingFilter : IActionFilter
{
    private readonly ILogger<ActionLoggingFilter> _logger;
    private readonly bool _logParameters;

    public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger,
        IOptions<AppOptions> options)
    {
        _logger = logger;
        _logParameters = options.Value.LogActionParameters;
    }
    public void OnActionExecuting(ActionExecutingContext context)
    { 
        _logger.LogTrace(String.Format("{0} - invoked", context.ActionDescriptor.DisplayName));
        if (_logParameters) 
            _logger.LogCritical(Strings.Format($"{LogParameters(context.ActionArguments)}"));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogTrace(string.Format("{0} - was finished", context.ActionDescriptor.DisplayName));
    }

    private string LogParameters(IDictionary<string, object> dict)
    {
        var res = string.Empty;
        if (dict.Count > 0)
        {
            res += "with parameters - ";
            foreach (var item in dict)
            {
                res += $"{item.Key}:{item.Value}\n";
            }
        }
        else
            res += "with no parameters\n";
        
        return res;
    }
}