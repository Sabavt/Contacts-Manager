using ContactsManager.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace ContactsManager.Filters.ActionFilters;

public class PersonsActionFilter : IAsyncActionFilter
{
    private readonly ILogger<PersonsActionFilter> _logger;

    public PersonsActionFilter(ILogger<PersonsActionFilter> logger)
    {
        _logger = logger;
    } 

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {   
        _logger.LogInformation("{Filter}.{Method}", nameof(PersonsActionFilter), nameof(OnActionExecutionAsync)); 
        
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
            }
            else
            { 
               context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
               _logger.LogInformation($"Updated Action parameter of searchBy is {nameof(PersonResponse.PersonName)}");
            }

        }

        await next();

        _logger.LogInformation("{Filter}.{Method}", nameof(PersonsActionFilter), nameof(OnActionExecutionAsync));

        var controller = (PersonsController)context.Controller;
        var arguments = (IDictionary<string, object?>?)context.ActionArguments;

        if (arguments != null)
        {
            if (arguments.ContainsKey("searchBy"))
            {
                controller.ViewBag.CurrentSearchBy = Convert.ToString(arguments["searchBy"]);
            }
            if (arguments.ContainsKey("searchString"))
            {
                controller.ViewBag.CurrentSearchString = Convert.ToString(arguments["searchString"]);
            }
            if (arguments.ContainsKey("sortBy"))
            {
                controller.ViewBag.CurrentSortBy = Convert.ToString(arguments["sortBy"]);
            }
            if (arguments.ContainsKey("sortOptions"))
            {
                controller.ViewBag.CurrentSortOptions = Convert.ToString(arguments["sortOptions"]);
            }
        }
        controller.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.Country), "Country" },
                { nameof(PersonResponse.Address), "Address" }
            };
    }
}
