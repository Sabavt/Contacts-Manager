using AutoFixture;
using Entities;
using FluentAssertions; 
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System.Linq.Expressions;

namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService; 
    private readonly IPersonsRepository _personsRepository;
    private readonly Mock<IPersonsRepository> _personsRepositoryMock;
    private readonly ITestOutputHelper _outputHelper; 
    private readonly IFixture _fixture;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    { 
        _personsRepositoryMock = new Mock<IPersonsRepository>();

        _personsRepository = _personsRepositoryMock.Object;

        _personsService = new PersonsService(_personsRepository); 
        _outputHelper = testOutputHelper;
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AddPerson_NullPerson_ToBeArgumentNullException()
    {  
        PersonAddRequest? request = null;
        Func<Task> act = async () => await _personsService.AddPerson(request);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
    {
        PersonAddRequest? request = new PersonAddRequest() { PersonName = null };

        var person = request.ToPerson();
        _personsRepositoryMock.Setup(t => t.AddPerson(It.IsAny<Person>()))
            .ReturnsAsync(person);

        Func<Task> act = async () => await _personsService.AddPerson(request);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddPerson_ProperPersonDetails_ToBeSuccessful()
    {
        PersonAddRequest request = _fixture.Build<PersonAddRequest>()  
            .With(p => p.Email, "john@gmail.com").Create();

        Person person = request.ToPerson();
        PersonResponse person_response_expected = person.ToPersonResponse();

        _personsRepositoryMock.Setup(tmp => 
        tmp.AddPerson(It.IsAny<Person>()))
            .ReturnsAsync(person);

        PersonResponse person_response_from_add = await _personsService.AddPerson(request);
        person_response_expected.PersonID = person_response_from_add.PersonID;

        person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
        person_response_from_add.Should().Be(person_response_expected);
    }

    [Fact]
    public async Task GetPersonByPersonID_NullPersonID_ToBeArgumentNullException()
    {
        Guid? guid = null;
        Func<Task> act = async () => await _personsService.GetPersonByPersonID(guid);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetPersonByPersonID_WithPersonID_ToBeSuccessful()
    {  
        PersonAddRequest person_request = new PersonAddRequest() { PersonName = "person", Email = "email@sample.com", Address = "address", DateOfBirth = DateTime.Parse("2020-01-01"), Gender = GenderOptions.Male, ReceiveNewsLetters = false };
        var person = person_request.ToPerson();

        _personsRepositoryMock.Setup(temp => temp
            .GetPersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(person
            );
         
        PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonID(person.PersonID);

        person.ToPersonResponse().Should().BeEquivalentTo(person_response_from_get);
    }


    [Fact]
    public async Task GetAllPerson_EmptyList_ToBeEmpty()
    { 
        _personsRepositoryMock.Setup(t => t.GetAllPersons()).ReturnsAsync(new List<Person>());
        List<PersonResponse> persons = await _personsService.GetAllPerson();

        persons.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllPerson_AfterFewPerson_ToBeSuccessful()
    { 
        PersonAddRequest add_request = new PersonAddRequest() { PersonName = "TestName", Address = "TestAddress", DateOfBirth = DateTime.Parse("2000-01-01"), Email = "test@gmail.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true }; 

        _personsRepositoryMock
            .Setup((t) => t.GetAllPersons()).ReturnsAsync(new List<Person>() { add_request.ToPerson()});

        _outputHelper.WriteLine(add_request.ToPerson().ToString() ?? "null");

        var allPersons = await _personsService.GetAllPerson();
        allPersons.ForEach(person => { _outputHelper.WriteLine(person.ToString()); } );

        allPersons.Should().Contain(add_request.ToPerson().ToPersonResponse());
    }

    [Fact] 
    public async Task GetFilteredPersons_EmptySearchText_ToBeListOfAllPersons()
    {  
        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith",   DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary",   DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Gio", Email = "gio@example.com", Gender = GenderOptions.Male, Address = "address of gio",   DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
        var persons = person_requests.Select(t => t.ToPerson()).ToList(); 

        _personsRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
          
        List<PersonResponse> persons_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");
         
        _outputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_search in persons_list_from_search)
        {
            _outputHelper.WriteLine(person_response_from_search.ToString());
        } 

        persons_list_from_search.Should().BeEquivalentTo(persons.Select(t => t.ToPersonResponse()));
    }

     
    [Fact]
    public async Task GetFilteredPersons_SearchByPersonName_ToBeFiltered()
    {
        PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

        PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

        PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Gio", Email = "gio@example.com", Gender = GenderOptions.Male, Address = "address of gio", DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

        List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };
        var persons = person_requests.Select(t => t.ToPerson()).ToList();

        _personsRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

        List<PersonResponse> persons_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

        _outputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_search in persons_list_from_search)
        {
            _outputHelper.WriteLine(person_response_from_search.ToString());
        }

        persons_list_from_search.Should().BeEquivalentTo(persons.Select(t => t.ToPersonResponse()));
    }

    [Fact]
    public async Task GetSortedPerson_ToBeSuccessful()
    {

        List<Person> persons = [_fixture.Build<Person>().With(t => t.Email, "TestPerson1@gmail.com").With(t => t.Country, null as Country).Create(), 
        _fixture.Build<Person>().With(t => t.Email, "TestPerson2@gmail.com").With(t => t.Country, null as Country).Create(), _fixture.Build<Person>().With(t => t.Email, "TestPerson3@gmail.com").With(t => t.Country, null as Country).Create()];  

        _personsRepositoryMock.Setup(m => m.GetAllPersons())
            .ReturnsAsync(persons);
         
          
        List<PersonResponse> allPersons = await _personsService.GetAllPerson();

        List<PersonResponse> persons_list_from_sort = await _personsService.GetSortedPerson(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

        _outputHelper.WriteLine("Actual:");
        foreach (PersonResponse person_response_from_get in persons_list_from_sort)
        {
            _outputHelper.WriteLine(person_response_from_get.ToString());
        }

        persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
    }

    [Fact]
    public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
    {
        PersonUpdateRequest? person_update_request = null;
        Func<Task> act = async () => await _personsService.UpdatePerson(person_update_request);
        
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
    {
        PersonUpdateRequest? person_update_request = new() { PersonID = Guid.NewGuid() };
         
        Func<Task> act = async () => await _personsService.UpdatePerson(person_update_request);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
    {
        var person = _fixture.Build<Person>().With(t => t.PersonName, null as string)
            .With(t => t.Email, "TestPerson2@gmail.com")
            .With(t => t.Country, null as Country)
            .Create();

        Func<Task> act = async () => await _personsService.UpdatePerson(person.ToPersonResponse().ToPersonUpdateRequest());

        await act.Should().ThrowAsync<ArgumentException>();
    }
    
    [Fact]
    public async Task UpdatePerson_ProperDetails_ToBeSuccessful()
    {
        var person = _fixture.Build<Person>() 
            .With(t => t.Email, "TestPerson@gmail.com")
            .With(t => t.Country, null as Country)
            .Create(); 

        _personsRepositoryMock.Setup(t => t.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);
        _personsRepositoryMock.Setup(t => t.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);
          
       var person_updated = await _personsService.UpdatePerson(person.ToPersonResponse().ToPersonUpdateRequest()); 

        person.PersonName.Should().Be(person_updated.PersonName);
    }

    [Fact]
    public async Task DeletePerson_ValidPersonID_ToBeSuccessful()
    {
        var person = _fixture.Build<Person>()
          .With(t => t.Email, "TestPerson@gmail.com")
          .With(t => t.Country, null as Country)
          .Create();

        _personsRepositoryMock.Setup(t => t.DeletePersonByPersonID(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        bool isDeleted = await _personsService.DeletePerson(person.PersonID);

        isDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeletePerson_InvalidPersonID()
    { 
        bool person_response_from_get = await _personsService.DeletePerson(Guid.NewGuid());

        person_response_from_get.Should().BeFalse();
    }
}