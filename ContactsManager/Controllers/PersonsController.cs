using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ContactsManager.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly ICountriesService _countriesService;
        private readonly IPersonsService _personsService;

        public PersonsController(ICountriesService countriesService , IPersonsService personsService)
        {
            _countriesService = countriesService;
            _personsService = personsService;
        }

        [Route("[action]")] 
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
        [Route("[action]")] 
        public IActionResult Create()
        {
            ViewBag.Countries = _countriesService.GetAllCountries()
                .Select(item => new SelectListItem() 
                { 
                    Text = item.CountryName, Value = item.CountryID.ToString()
                }); 

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _countriesService.GetAllCountries().Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryID.ToString() }); 
                return View();
            }
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
            return RedirectToActionPermanent("Index");
        }

        [Route("[action]/{personID:Guid}")]
        public IActionResult Edit(Guid personID)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);
            if (personResponse == null)
                return NotFound();
            ViewBag.Countries = _countriesService.GetAllCountries().Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryID.ToString() });
            return View(personResponse);
        }
    }
}