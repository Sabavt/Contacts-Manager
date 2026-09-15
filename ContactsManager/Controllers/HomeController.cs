using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace ContactsManager.Controllers
{
    public class HomeController : Controller
    {
        [Route("/Error")]
        public IActionResult Error()
        {
            IExceptionHandlerFeature? feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if(feature != null && feature.Error != null)
            {
                ViewBag.Error = feature.Error.Message;
            }
            return View();
        }
    }
}
