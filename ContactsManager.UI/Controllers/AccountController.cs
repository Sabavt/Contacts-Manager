using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers;

public class AccountController : Controller
{
    public IActionResult Register()
    {
        return View();
    }
}
