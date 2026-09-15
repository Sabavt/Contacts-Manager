using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Controllers
{
    public class HomeController : Controller
    {
        [Route("/Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
