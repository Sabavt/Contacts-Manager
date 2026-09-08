using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
