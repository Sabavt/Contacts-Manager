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
        PersonsController personsController = new PersonsController(_countriesService, _personsService);
        PersonAddRequest person_to_add = _fixture.Create<PersonAddRequest>();
         
        _personsServiceMock.Setup(t => t.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(new PersonResponse());

        var result = await personsController.Create(person_to_add);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

        redirectToActionResult.ActionName.Should().Be("Index");
    }

    [Fact]
    public async Task Create_ValidationErrors_ToReturnCreateView()
    {
        PersonsController personsController = new PersonsController(_countriesService, _personsService);
        PersonAddRequest person_to_add = _fixture.Create<PersonAddRequest>();
        PersonResponse person_response = _fixture.Create<PersonResponse>();
        List<CountryResponse> country_response = _fixture.Create<List<CountryResponse>>();

        _personsServiceMock.Setup(t => t.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(person_response);
        _countriesServiceMock.Setup(t => t.GetAllCountries())
            .ReturnsAsync(country_response);

        personsController.ModelState.AddModelError("Country", "Country value is not valid");

        var result = await personsController.Create(person_to_add);
      
        var view = Assert.IsType<ViewResult>(result);

        view.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
        view.ViewData.Should().NotBeNull();
        view.ViewData.Model.Should().Be(person_to_add);
        
    }

    [Fact]
    public async Task Edit_ValidModelState_ToBeRedirectedIndexView()
    {
        var person_update_mock = _fixture.Create<PersonUpdateRequest>();
        var person_response_mock = _fixture.Create<PersonResponse>(); 
        _personsServiceMock.Setup(t => t.UpdatePerson(It.IsAny<PersonUpdateRequest>()))
            .ReturnsAsync(person_response_mock); 
        PersonsController persons = new PersonsController(_countriesService, _personsService);

        var result = Assert.IsType<RedirectToActionResult>(await persons.Edit(person_update_mock)); 
        result.ActionName.Should().Be("Index");
    }

    [Fact]
    public async Task Edit_InvalidModelState_ToBeViewWithErrors()
    {
        var person_update_mock = _fixture.Create<PersonUpdateRequest>();
        var country_response_mock = _fixture.Create<List<CountryResponse>>();
        _countriesServiceMock.Setup(t => t.GetAllCountries())
            .ReturnsAsync(country_response_mock);
        PersonsController persons = new PersonsController(_countriesService, _personsService);

        persons.ModelState.AddModelError("UpdateRequest", "Invalid Update Request");

        var result = Assert.IsType<ViewResult>(await persons.Edit(person_update_mock));
        result.ViewData.Model.Should().Be(person_update_mock);
        result.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
    }

    [Fact]
    public async Task Delete_ValidModelState_ToBeRedirectedIndexView()
    {
        var person_delete_mock = _fixture.Create<PersonUpdateRequest>(); 
        PersonsController persons = new PersonsController(_countriesService, _personsService);
        _personsServiceMock.Setup(t => t.DeletePerson(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var result = Assert.IsType<RedirectToActionResult>(await persons.Delete(person_delete_mock));
        result.ActionName.Should().Be("Index");
    }

    [Fact]
    public async Task Delete_InvalidModelState_ToBeViewWithErrors()
    {
        var person_delete_mock = _fixture.Create<PersonUpdateRequest>(); 
        PersonsController persons = new PersonsController(_countriesService, _personsService);

        persons.ModelState.AddModelError("Delete", "Invalid Delete Request");

        var result = Assert.IsType<ViewResult>(await persons.Delete(person_delete_mock));
        result.ViewData.Model.Should().Be(person_delete_mock);
        result.ViewData.Model.Should().BeAssignableTo<PersonUpdateRequest>();
    }
}
