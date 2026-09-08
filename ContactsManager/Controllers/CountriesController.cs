using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        [Route("[action]")]
        public IActionResult UploadExcel()
        {
            return View();
        }
    }
}
