using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions; 

namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _outputHelper;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _personsService = new PersonsService();
        _countriesService = new CountriesService();
        _outputHelper = testOutputHelper;
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
        PersonAddRequest? request = new PersonAddRequest() { PersonName = null };

        Assert.Throws<ArgumentException>(() => _personsService.AddPerson(request));
    }

    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
        PersonAddRequest request = new PersonAddRequest() { PersonName = "Test", Address = "Test", CountryID = Guid.NewGuid(), Email = "test@gmail.com", Gender =  GenderOptions.Female, ReceiveNewsLetters = true };

        PersonResponse person_response_from_add = _personsService.AddPerson(request);


        Assert.Contains(person_response_from_add, _personsService.GetAllPerson());
        Assert.True(person_response_from_add.PersonID != Guid.Empty);
    }

    [Fact]
    public void GetPersonByPersonID_NullPersonID()
    {
        Guid? guid = null;

        Assert.Throws<ArgumentNullException>(() => _personsService.GetPersonByPersonID(guid));
    }

    [Fact]
    public void GetPersonByPersonID_WithPersonID()
    {
        CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };
        CountryResponse country_response = _countriesService.AddCountry(country_request);

        PersonAddRequest person_request = new PersonAddRequest() { PersonName = "person", Email = "email@sample.com", Address = "address", CountryID = country_response.CountryID, DateOfBirth = DateTime.Parse("2020-01-01"), Gender = GenderOptions.Male, ReceiveNewsLetters = false };

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
        CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };
        CountryResponse country_response = _countriesService.AddCountry(country_request);
        PersonAddRequest add_request = new PersonAddRequest() { PersonName = "TestName", Address = "TestAddress", CountryID = country_response.CountryID, DateOfBirth = DateTime.Parse("2000-01-01"), Email = "test@gmail.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

        PersonResponse response_from_add = _personsService.AddPerson(add_request);

        _outputHelper.WriteLine(response_from_add.ToString());
        _personsService.GetAllPerson().ForEach(person => { _outputHelper.WriteLine(person.ToString()); } );

        Assert.Contains(response_from_add, _personsService.GetAllPerson());
    }

    [Fact] 
    public void GetFilteredPersons_EmptySearchText()
    { 
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Georgia" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Gio", Email = "gio@example.com", Gender = GenderOptions.Male, Address = "address of gio", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }
        
        _outputHelper.WriteLine("Expected:");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _outputHelper.WriteLine(person_response_from_add.ToString());
        }
         
        List<PersonResponse> persons_list_from_search = _personsService.GetFilteredPersons(nameof(Person.PersonName), "");
         
        _outputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in persons_list_from_search)
        {
            _outputHelper.WriteLine(person_response_from_get.ToString());
        }
         
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, persons_list_from_search);
        }
    }

     
    [Fact]
    public void GetFilteredPersons_SearchByPersonName()
    { 
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Georgia" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Gio", Email = "gio@example.com", Gender = GenderOptions.Male, Address = "address of gio", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }
         
        _outputHelper.WriteLine("Expected:");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _outputHelper.WriteLine(person_response_from_add.ToString());
        }
         
        List<PersonResponse> persons_list_from_search = _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");
         
        _outputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in persons_list_from_search)
        {
            _outputHelper.WriteLine(person_response_from_get.ToString());
        }
         
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            if (person_response_from_add.PersonName != null)
            {
                if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(person_response_from_add, persons_list_from_search);
                }
            }
        }
    }

    [Fact]
    public void GetSortedPerson()
    {
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Georgia" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Gio", Email = "gio@example.com", Gender = GenderOptions.Male, Address = "address of gio", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

        List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in person_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            person_response_list_from_add.Add(person_response);
        }

        _outputHelper.WriteLine("Expected:");
        foreach (PersonResponse person_response_from_add in person_response_list_from_add)
        {
            _outputHelper.WriteLine(person_response_from_add.ToString());
        }

        List<PersonResponse> allPersons = _personsService.GetAllPerson();

        List<PersonResponse> persons_list_from_sort = _personsService.GetSortedPerson(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

        _outputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in persons_list_from_sort)
        {
            _outputHelper.WriteLine(person_response_from_get.ToString());
        }

        person_response_list_from_add = person_response_list_from_add.OrderByDescending(x => x.PersonName).ToList();

        for (int i = 0; i < person_response_list_from_add.Count; i++)
        {
            Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
        }
    }

    [Fact]
    public void UpdatePerson_NullPerson()
    {
        PersonUpdateRequest? person_update_request = null;
        Assert.Throws<ArgumentNullException>(() => _personsService.UpdatePerson(person_update_request));
    }
    
    [Fact]
    public void UpdatePerson_InvalidPersonID()
    {
        PersonUpdateRequest? person_update_request = new() { PersonID = Guid.NewGuid() };
        Assert.Throws<ArgumentException>(() => _personsService.UpdatePerson(person_update_request));
    }

    [Fact]
    public void UpdatePerson_PersonNameIsNull()
    {
        CountryAddRequest? country_add_request = new() { CountryName = "UK"};
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        PersonAddRequest person_add_request = new() { PersonName = "test", Email = "test@gmail.com", CountryID = country_response_from_add.CountryID };
        PersonResponse? person_response_from_add = _personsService.AddPerson(person_add_request);

        PersonUpdateRequest? person_update_request = person_response_from_add.ToPersonUpdateRequest();
        person_update_request.PersonName = null;

        Assert.Throws<ArgumentException>(() => _personsService.UpdatePerson(person_update_request)); 
    }
    
    [Fact]
    public void UpdatePerson_ProperDetails()
    {
        CountryAddRequest? country_add_request = new() { CountryName = "UK"};
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        PersonAddRequest person_add_request = new() { PersonName = "test", Email = "test@gmail.com", CountryID = country_response_from_add.CountryID };
        PersonResponse? person_response_from_add = _personsService.AddPerson(person_add_request);

        PersonUpdateRequest? person_update_request = person_response_from_add.ToPersonUpdateRequest(); 
        person_update_request.PersonName = "Larry";

        PersonResponse person_response_from_update = _personsService.UpdatePerson(person_update_request);
        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(person_response_from_update.PersonID);

        Assert.Equal(person_response_from_update, person_response_from_get);
    }

    [Fact]
    public void DeletePerson_ValidPersonID()
    {
        CountryAddRequest? country_add_request = new() { CountryName = "UK" };
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        PersonAddRequest person_add_request = new() { PersonName = "test", Email = "test@gmail.com", CountryID = country_response_from_add.CountryID };
        PersonResponse? person_response_from_add = _personsService.AddPerson(person_add_request);

        Assert.True(_personsService.DeletePerson(person_response_from_add.PersonID));
    }
    [Fact]
    public void DeletePerson_InvalidPersonID()
    {
        CountryAddRequest? country_add_request = new() { CountryName = "UK" };
        CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

        Assert.False(_personsService.DeletePerson(Guid.NewGuid()));
    }
}