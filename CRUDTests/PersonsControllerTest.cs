using AutoFixture;
using Moq;
using ServiceContracts;
using FluentAssertions;
using ContactsManager.Controllers;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CRUDTests;

public class PersonsControllerTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    private readonly Mock<IPersonsService> _personsServiceMock;
    private readonly Mock<ICountriesService> _countriesServiceMock;
    private readonly Fixture _fixture;
    public PersonsControllerTest()
    {
        _fixture = new Fixture();
        _personsServiceMock = new Mock<IPersonsService>();
        _countriesServiceMock = new Mock<ICountriesService>();
        _personsService = _personsServiceMock.Object;
        _countriesService = _countriesServiceMock.Object;
    }

    [Fact]
    public async Task Index_ReturnsPersonsView_ToBeSuccess()
    {
        List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();

        PersonsController personsController = new PersonsController(_countriesService, _personsService);

        _personsServiceMock.Setup(t => t.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new List<PersonResponse>());
        _personsServiceMock.Setup(t => t.GetSortedPerson(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(new List<PersonResponse>());

       IActionResult result = await personsController.Index(
           _fixture.Create<string>(), _fixture.Create<string>(),
           _fixture.Create<string>(), _fixture.Create<SortOrderOptions>()
       );

       ViewResult view = Assert.IsType<ViewResult>(result);

        view.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
        view.ViewData.Model.Should().Be(persons_response_list);
    }

}
