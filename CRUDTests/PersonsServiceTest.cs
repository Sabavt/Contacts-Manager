using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    public PersonsServiceTest()
    {
        _personsService = new PersonsService();
        _countriesService = new CountriesService();
    }

    [Fact]
    public void AddPerson_NullPerson()
    {
        PersonAddRequest? request = null;

        Assert.Throws<ArgumentNullException>(() => _personsService.AddPerson(request)); 
    }
    
    [Fact]
    public void AddPerson_PersonNameIsNull()
    {
        PersonAddRequest? request = new PersonAddRequest() { PersonName = null};

        Assert.Throws<ArgumentException>(() => _personsService.AddPerson(request)); 
    }

    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
        PersonAddRequest request = new PersonAddRequest() { PersonName = "Test", Address = "Test", CountryID = Guid.NewGuid(), Email = "test", Gender = ServiceContracts.Enums.GenderOptions.Female, ReceiveNewsLetters = true};

        PersonResponse person_response_from_add = _personsService.AddPerson(request);


        Assert.Contains(person_response_from_add, _personsService.GetAllPerson()); 
        Assert.True(person_response_from_add.PersonID != Guid.Empty);
    }

    [Fact]
    public void GetPersonByPersonID_NullPerson()
    {
        Guid? guid = null;

        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(guid);

        Assert.Null(person_response_from_get);
    }

    [Fact]
    public void GetPersonByPersonID_WithPersonID()
    {
        CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };
        CountryResponse country_response = _countriesService.AddCountry(country_request);

        PersonAddRequest person_request = new PersonAddRequest() { PersonName = "person name...", Email = "email@sample.com", Address = "address", CountryID = country_response.CountryID, DateOfBirth = DateTime.Parse("2000-01-01"), Gender = GenderOptions.Male, ReceiveNewsLetters = false };

        PersonResponse person_response_from_add = _personsService.AddPerson(person_request);

        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(person_response_from_add.PersonID);
         
        Assert.Equal(person_response_from_add, person_response_from_get);
    }


    [Fact]
    public void GetAllPerson_EmptyList()
    {
        Assert.Empty(_personsService.GetAllPerson());
    }

    [Fact]
    public void GetAllPerson_AfterFewPerson()
    {

    }
}
