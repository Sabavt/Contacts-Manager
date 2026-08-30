using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;

    public PersonsServiceTest()
    {
        _personsService = new PersonsService();
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
}
