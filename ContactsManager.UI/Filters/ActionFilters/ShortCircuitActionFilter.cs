using ContactsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace ContactsManager.Filters.ActionFilters;

public class ShortCircuitActionFilter : IAsyncActionFilter
{
    private readonly ICountriesGetterService _countriesGetterService;

    public ShortCircuitActionFilter(ICountriesGetterService countriesGetterService)
    {
        _countriesGetterService = countriesGetterService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is PersonsController controller)
        {
            if (!context.ModelState.IsValid)
            {
                controller.ViewBag.Countries = _countriesGetterService.GetAllCountries().Result.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryID.ToString() });
                controller.ViewBag.ErrorMessages = controller.ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                context.Result = controller.View(context.ActionArguments["personRequest"]);
            }
            else
            {
                await next();
            }
        }

    }
}
