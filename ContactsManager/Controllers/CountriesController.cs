using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace ContactsManager.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    { 
        private readonly ICountriesAdderService _countriesService;

        public CountriesController(ICountriesAdderService countriesService)
        {
            _countriesService = countriesService;
        }

        [Route("[action]")]
        public IActionResult UploadExcel()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UploadExcel(IFormFile excelFile)
        {
            if(excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select a valid Excel file to upload.";
                return View();
            }

            if(Path.GetExtension(excelFile.FileName).ToLower() != ".xlsx")
            {
                ViewBag.ErrorMessage = "Invalid file format. Please upload an Excel file with .xlsx extension.";
                return View();
            }

            int uploaded_countries = await _countriesService.UploadCountriesFromExcel(excelFile);
            ViewBag.Message = $"{uploaded_countries} countries have been successfully uploaded from the Excel file.";
            return View();
        }
    }
}
