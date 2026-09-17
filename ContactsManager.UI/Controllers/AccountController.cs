using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        if(!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(e => e.Errors).Select(v => v.ErrorMessage).ToList();
            return View(registerRequest);
        }

        ApplicationUser user = new()
        {
            Email = registerRequest.Email ,
            UserName = registerRequest.Email,
            PersonName = registerRequest.PersonName,
            PhoneNumber = registerRequest.Phone 
        };

        
        var result = await _userManager.CreateAsync(user);

        if(result.Succeeded)
        {
            return RedirectToActionPermanent("Index", "Persons");  
        }
        else
        {

         foreach(var error in result.Errors)
         {
            ModelState.AddModelError("Account Register" ,error.Description);
         }

             return View(registerRequest);
        }
    }
}
