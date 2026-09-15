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
    private readonly IPersonsSetterService _personsService;
    private readonly ICountriesService _countriesService; 
    private readonly ILogger<PersonsController> _logger;
    private readonly Mock<IPersonsSetterService> _personsServiceMock;
    private readonly Mock<ICountriesService> _countriesServiceMock;
    private readonly Mock<ILogger<PersonsController>> _loggerMock;
    private readonly Fixture _fixture;


    public PersonsControllerTest()
    {
        _fixture = new Fixture();
        _personsServiceMock = new Mock<IPersonsSetterService>();
        _countriesServiceMock = new Mock<ICountriesService>();
        _loggerMock = new Mock<ILogger<PersonsController>>();
        _personsService = _personsServiceMock.Object;
        _countriesService = _countriesServiceMock.Object;
        _logger = _loggerMock.Object;
    }

    [Fact]
    public async Task Index_ReturnsPersonsView_ToBeSuccess()
    {
        List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();

        PersonsController personsController = new PersonsController(_countriesService, _personsService, _logger);

        _personsServiceMock.Setup(t => t.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(persons_response_list);
        _personsServiceMock.Setup(t => t.GetSortedPerson(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(persons_response_list);

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
        PersonsController personsController = new PersonsController(_countriesService, _personsService, _logger);
        PersonAddRequest person_to_add = _fixture.Create<PersonAddRequest>();
         
        _personsServiceMock.Setup(t => t.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(new PersonResponse());

        var result = await personsController.Create(person_to_add);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

        redirectToActionResult.ActionName.Should().Be("Index");
    } 
}
