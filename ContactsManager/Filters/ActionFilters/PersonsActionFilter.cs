using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace ContactsManager.Filters.ActionFilters;

public class PersonsActionFilter : IActionFilter
{
    private readonly ILogger<PersonsActionFilter> _logger;

    public PersonsActionFilter(ILogger<PersonsActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("PersonsActionFilter OnActionExecuted");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("PersonsActionFilter OnActionExecuting");
        var searchBy = new List<string>()
        {
             nameof(PersonResponse.PersonName),
             nameof(PersonResponse.Email),
             nameof(PersonResponse.DateOfBirth),
             nameof(PersonResponse.Gender),
             nameof(PersonResponse.Country),
             nameof(PersonResponse.Address)
        };

        if (context.ActionArguments.ContainsKey("searchBy"))
        {
            var searchByArgument = Convert.ToString(context.ActionArguments["searchBy"]);

            if (!string.IsNullOrEmpty(searchByArgument))
            {
                if (searchBy.Any(t => t.Equals(searchByArgument)))
                {
                    _logger.LogInformation("Action parameter of searchBy is {searchByArgument}", searchByArgument);
                }
            context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
            } 

            _logger.LogInformation($"Updated Action parameter of searchBy is {nameof(PersonResponse.PersonName)}"); 
        }
    }
}
