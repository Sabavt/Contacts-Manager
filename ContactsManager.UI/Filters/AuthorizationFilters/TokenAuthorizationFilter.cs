using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.Filters.AuthorizationFilters;

public class TokenAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if(context.HttpContext.Request.Cookies["Auth-Key"] != "CookieForContactsManager")
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);  
        }
        if(!context.HttpContext.Request.Cookies.ContainsKey("Auth-Key"))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        } 
    }
}
