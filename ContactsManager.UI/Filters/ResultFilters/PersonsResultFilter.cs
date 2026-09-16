using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.ResultFilters;

public class PersonsResultFilter : IAsyncResultFilter
{
    private readonly ILogger<PersonsResultFilter> _logger;

    public PersonsResultFilter(ILogger<PersonsResultFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        _logger.LogInformation("{Filter}.{Method}", nameof(PersonsResultFilter), nameof(OnResultExecutionAsync));
        context.HttpContext.Request.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy mm dd hh:mm");

        await next();

        _logger.LogInformation("{Filter}.{Method} after", nameof(PersonsResultFilter), nameof(OnResultExecutionAsync));
    }
}
