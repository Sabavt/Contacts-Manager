using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    [HttpGet]
    [Route("[action]")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Register(RegisterRequest registerRequest)
    {
        ApplicationUser user = new()
        {
            Email = registerRequest.Email

        };
        return RedirectToActionPermanent("Index", "Persons"); 
    }
}
