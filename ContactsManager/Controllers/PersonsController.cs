using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace ContactsManager.Controllers
{
    public class PersonsController : Controller
    {
        private readonly ICountriesService _countriesService;
        private readonly IPersonsService _personsService;

        public PersonsController(ICountriesService countriesService , IPersonsService personsService)
        {
            _countriesService = countriesService;
            _personsService = personsService;
        }

        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.Country), "Country" },
                { nameof(PersonResponse.Address), "Address" }
            };
            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrectSearchBy = searchBy;
            ViewBag.CurrectSearchString = searchString;

            return View(persons);
        }
    }
}
