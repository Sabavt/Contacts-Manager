using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
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

        public PersonsController(ICountriesService countriesService, IPersonsService personsService)
        {
            _countriesService = countriesService;
            _personsService = personsService;
        }

        [Route("[action]")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOptions = SortOrderOptions.ASC)
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
            List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            List<PersonResponse> sorted_persons = await _personsService.GetSortedPerson(persons, sortBy, sortOptions);

            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOptions = sortOptions.ToString();

            return View(sorted_persons);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            var countries = await _countriesService.GetAllCountries();

            ViewBag.Countries = countries
                .Select(item => new SelectListItem()
                {
                    Text = item.CountryName,
                    Value = item.CountryID.ToString()
                });

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _countriesService.GetAllCountries().Result.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryID.ToString() });
                ViewBag.ErrorMessages = ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return View();
            }
            PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);
            return RedirectToActionPermanent("Index");
        }

        [HttpGet]
        [Route("[action]/{personID:Guid}")]
        public async Task<IActionResult> Edit(Guid? personID)
        {
            var person = await _personsService.GetPersonByPersonID(personID);
            PersonUpdateRequest? person_update_get = person?.ToPersonUpdateRequest();
            if (person_update_get == null)
                return RedirectToActionPermanent("Index");

            ViewBag.Countries = _countriesService.GetAllCountries().Result.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryID.ToString() });
            return View(person_update_get);
        }

        [HttpPost]
        [Route("[action]/{personID:guid}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        { 
            if (!ModelState.IsValid)
            { 
                ViewBag.Countries = _countriesService.GetAllCountries().Result.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryID.ToString() }); 
                ViewBag.ErrorMessages = ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return View(personUpdateRequest);
            }
            PersonResponse? person_response = await _personsService.UpdatePerson(personUpdateRequest);
            if (person_response == null)
                return RedirectToActionPermanent("Index");

            return RedirectToActionPermanent("Index");
        }

        [HttpGet]
        [Route("[action]/{personID:Guid}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? person_delete_get = await _personsService.GetPersonByPersonID(personID);
            if (person_delete_get == null)
                return RedirectToActionPermanent("Index");
             
            return View(person_delete_get);
        }

        [HttpPost]
        [Route("[action]/{personID:guid}")]
        public async Task<IActionResult> Delete(PersonResponse personUpdateRequest)
        {
            if (!ModelState.IsValid)
            { 
                ViewBag.ErrorMessages = ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return View();
            }
            bool IsDeleted = await _personsService.DeletePerson(personUpdateRequest.PersonID);

            if (IsDeleted is true)
                return RedirectToActionPermanent("Index");

            return RedirectToActionPermanent("Index");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> personResponses = await  _personsService.GetAllPerson();

            return new ViewAsPdf("PersonsPDF", personResponses, ViewData)
            {
                PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}