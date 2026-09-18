using ContactsManager.Controllers;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.UI.Controllers;

[Route("[controller]")] 
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;  

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    [Route("[action]")]
    [Authorize("NotAuthorized")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    [Authorize("NotAuthorized")] 
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(e => e.Errors).Select(v => v.ErrorMessage).ToList();
            return View(registerRequest);
        }

        ApplicationUser user = new()
        {
            Email = registerRequest.Email,
            UserName = registerRequest.Email,
            PersonName = registerRequest.PersonName,
            PhoneNumber = registerRequest.Phone  
        };


        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (result.Succeeded)
        {
            if(registerRequest.UserType == UserTypeOptions.Admin)
            {
                if (await _roleManager.FindByNameAsync(nameof(UserTypeOptions.Admin)) is null)
                {
                    ApplicationRole applicationRole = new ApplicationRole() { Name = nameof(UserTypeOptions.Admin) };
                    await _roleManager.CreateAsync(applicationRole);
                }
                await _userManager.AddToRoleAsync(user, nameof(UserTypeOptions.Admin));
            }
            else
            { 
                await _userManager.AddToRoleAsync(user, nameof(UserTypeOptions.User));
            }
            await _signInManager.SignInAsync(user, true);
            return RedirectToActionPermanent("Index", "Persons");
        }
        else
        { 
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Account Register", error.Description);
            } 

            return View(registerRequest);
        }
    }

    [HttpGet]
    [Route("[action]")]
    [Authorize("NotAuthorized")] 
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    [Authorize("NotAuthorized")]
    public async Task<IActionResult> Login(LoginRequest loginRequest, string? returnUrl)
    {
        if(!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(v => v.ErrorMessage).ToList();
            return View(loginRequest);
        }

        var result = await _signInManager.PasswordSignInAsync(loginRequest.Email!, loginRequest.Password!, isPersistent:true, lockoutOnFailure:false);

        if(result.Succeeded)
        {
            if(string.IsNullOrEmpty(returnUrl))
            {
              return RedirectToActionPermanent(nameof(PersonsController.Index),"Persons"); 
            }
            else
            {
                return LocalRedirectPermanent(returnUrl); 
            }
        }
        else
        {
            ModelState.AddModelError("Login", "Invalid email or password");
            return View(loginRequest);
        }
    }

    [Route("[action]")]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToActionPermanent(nameof(PersonsController.Index), "persons");
    }

    [Route("[action]")]
    public async Task<IActionResult> EmailAlredyExists(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null)
        {
            return Json(true);
        }
        else
        {
            return Json(false);
        }
    }

    [Route("[action]")] 
    public async Task<IActionResult> PhoneAlredyExists(string phone)
    {
        bool exists = await _userManager.Users.AnyAsync(u => u.PhoneNumber == phone);

        if(exists)
        {
            return Json(false);
        }
        else
        {
            return Json(true);
        }
    }
}
