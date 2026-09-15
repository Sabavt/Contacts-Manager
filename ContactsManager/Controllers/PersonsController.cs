using ContactsManager.Filters.ActionFilters;
using ContactsManager.Filters.AuthorizationFilters;
using ContactsManager.Filters.ExceptionFilters;
using ContactsManager.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ContactsManager.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(HandleExceptionFilter))]
    public class PersonsController : Controller
    {
        private readonly ICountriesService _countriesService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService; 
        private readonly ILogger<PersonsController> _logger;

        public PersonsController(IPersonsGetterService personsGetterService, IPersonsSorterService personsSorterService, IPersonsUpdaterService personsUpdaterService, IPersonsDeleterService personsDeleterService, IPersonsAdderService personsAdderService,ICountriesService countriesService, ILogger<PersonsController> logger)
        {
            _personsGetterService = personsGetterService;
            _personsSorterService = personsSorterService;
            _personsUpdaterService = personsUpdaterService;
            _personsDeleterService = personsDeleterService;
            _personsAdderService = personsAdderService;

            _countriesService = countriesService; 
            _logger = logger;
        }

        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsActionFilter))]
        [TypeFilter(typeof(PersonsResultFilter))]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOptions = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index method of PersonsController");
            _logger.LogDebug($"searchBy : {searchBy}, searchString : {searchString}, sortBy : {sortBy}, sortOptions : {sortOptions}");
             
            List<PersonResponse> persons = await _personsGetterService.GetFilteredPersons(searchBy, searchString); 

            List<PersonResponse> sorted_persons = await _personsSorterService.GetSortedPerson(persons, sortBy, sortOptions); 

            return View(sorted_persons);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Create method of PersonsController");
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
        [TypeFilter(typeof(ShortCircuitActionFilter))] 
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        { 
            PersonResponse personResponse = await _personsAdderService.AddPerson(personRequest);
            return RedirectToActionPermanent("Index");
        }

        [HttpGet]
        [Route("[action]/{personID:Guid}")]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Edit(Guid? personID)
        {
            var person = await _personsGetterService.GetPersonByPersonID(personID);
            PersonUpdateRequest? person_update_get = person?.ToPersonUpdateRequest();
            if (person_update_get == null)
                return RedirectToActionPermanent("Index");

            ViewBag.Countries = _countriesService.GetAllCountries().Result.Select(item => new SelectListItem() { Text = item.CountryName, Value = item.CountryID.ToString() });
            return View(person_update_get);
        }

        [HttpPost]
        [Route("[action]/{personID:guid}")]
        [TypeFilter(typeof(ShortCircuitActionFilter))]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {  
            PersonResponse person_response = await _personsUpdaterService.UpdatePerson(personRequest);  
            return RedirectToActionPermanent("Index");
        }

        [HttpGet]
        [Route("[action]/{personID:Guid}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? person_delete_get = await _personsGetterService.GetPersonByPersonID(personID);
            if (person_delete_get == null)
                return RedirectToActionPermanent("Index");
             
            return View(person_delete_get);
        }

        [HttpPost]
        [Route("[action]/{personID:guid}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            if (!ModelState.IsValid)
            { 
                ViewBag.ErrorMessages = ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return View(personUpdateRequest);
            }
            bool IsDeleted = await _personsDeleterService.DeletePerson(personUpdateRequest.PersonID);

            if (IsDeleted is true)
                return RedirectToActionPermanent("Index");

            return RedirectToActionPermanent("Index");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> personResponses = await  _personsGetterService.GetAllPerson();

            return new ViewAsPdf("PersonsPDF", personResponses, ViewData)
            {
                PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream persons_csv_stream = await _personsGetterService.GetPersonsCSV();
            return File(persons_csv_stream, "text/csv", "Persons.csv");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream persons_excel_stream = await _personsGetterService.GetPersonsExcel();
            return File(persons_excel_stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Persons.xlsx");
        }
    }
}