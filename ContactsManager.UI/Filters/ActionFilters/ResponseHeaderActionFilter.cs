using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ActionFilters;

public class ResponseHeaderActionFilter : IAsyncActionFilter
{
    private readonly ILogger<ResponseHeaderActionFilter> _logger;
    private string _key;
    private string _value;

    public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value)
    {
        _logger = logger;
        _key = key;
        _value = value;
    }
     

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("{FilterName}.{ActionName}", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
        context.HttpContext.Response.Headers[_key] = _value;

       await next();

       _logger.LogInformation("{FilterName}.{ActionName}", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
    }
}
