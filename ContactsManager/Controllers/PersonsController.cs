using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

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
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOptions = SortOrderOptions.ASC)
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

            List<PersonResponse> sorted_persons = _personsService.GetSortedPerson(persons, sortBy, sortOptions);

            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOptions = sortOptions.ToString();

            return View(sorted_persons);
        }

        [HttpGet]
        [Route("persons/create")]
        public IActionResult Create()
        {
            ViewBag.Countries = _countriesService.GetAllCountries();
            return View();
        }

        [HttpPost]
        [Route("persons/create")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _countriesService.GetAllCountries();
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).ToList().Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index");
        }
    }
}
