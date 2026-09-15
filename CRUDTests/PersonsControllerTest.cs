using AutoFixture;
using Moq;
using ServiceContracts;
using FluentAssertions;
using ContactsManager.Controllers;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRUDTests;

public class PersonsControllerTest
{
    private readonly IPersonsAdderService _personsAdderService;
    private readonly IPersonsGetterService _personsGetterService;
    private readonly IPersonsSorterService _personsSorterService;
    private readonly IPersonsUpdaterService _personsUpdaterService;
    private readonly IPersonsDeleterService _personsDeleterService; 
    private readonly Mock<IPersonsAdderService> _personsAdderServiceMock;
    private readonly Mock<IPersonsGetterService> _personsGetterServiceMock;
    private readonly Mock<IPersonsSorterService> _personsSorterServiceMock;
    private readonly Mock<IPersonsUpdaterService> _personsUpdaterServiceMock;
    private readonly Mock<IPersonsDeleterService> _personsDeleterServiceMock;
    private readonly ICountriesAdderService _countriesService; 
    private readonly ILogger<PersonsController> _logger;
    private readonly Mock<ICountriesAdderService> _countriesServiceMock;
    private readonly Mock<ILogger<PersonsController>> _loggerMock;
    private readonly Fixture _fixture;


    public PersonsControllerTest()
    {
        _fixture = new Fixture();
        _personsAdderServiceMock = new Mock<IPersonsAdderService>();
        _personsGetterServiceMock = new Mock<IPersonsGetterService>();
        _personsSorterServiceMock = new Mock<IPersonsSorterService>();
        _personsUpdaterServiceMock = new Mock<IPersonsUpdaterService>();
        _personsDeleterServiceMock = new Mock<IPersonsDeleterService>();
        _countriesServiceMock = new Mock<ICountriesAdderService>();
        _loggerMock = new Mock<ILogger<PersonsController>>();

        _personsAdderService = _personsAdderServiceMock.Object;
        _personsDeleterService = _personsDeleterServiceMock.Object;
        _personsGetterService = _personsGetterServiceMock.Object;
        _personsSorterService = _personsSorterServiceMock.Object;
        _personsUpdaterService = _personsUpdaterServiceMock.Object;
        _countriesService = _countriesServiceMock.Object;
        _logger = _loggerMock.Object;
    }

    [Fact]
    public async Task Index_ReturnsPersonsView_ToBeSuccess()
    {
        List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();

        PersonsController personsController = new PersonsController(_personsGetterService, _personsSorterService, _personsUpdaterService, _personsDeleterService, _personsAdderService, _countriesService, _logger);

        _personsGetterServiceMock.Setup(t => t.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(persons_response_list);
        _personsSorterServiceMock.Setup(t => t.GetSortedPerson(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(persons_response_list);

       IActionResult result = await personsController.Index(
           _fixture.Create<string>(), _fixture.Create<string>(),
           _fixture.Create<string>(), _fixture.Create<SortOrderOptions>()
       );

       ViewResult view = Assert.IsType<ViewResult>(result);

        view.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
        view.ViewData.Model.Should().Be(persons_response_list);
    }

    [Fact]
    public async Task Create_NoValidationErrors_ToRedirectToIndex()
    {
        PersonsController personsController = new PersonsController(_personsGetterService, _personsSorterService, _personsUpdaterService, _personsDeleterService, _personsAdderService, _countriesService, _logger);
        PersonAddRequest person_to_add = _fixture.Create<PersonAddRequest>();
         
        _personsAdderServiceMock.Setup(t => t.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(new PersonResponse());

        var result = await personsController.Create(person_to_add);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

        redirectToActionResult.ActionName.Should().Be("Index");
    } 
}
