using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly List<Person> _people;
    private readonly ICountriesService _countries;

    public PersonsService(bool initialize = true)
    {
        _people = new List<Person>();
        _countries = new CountriesService();
        if (initialize)
        { 
            _people.Add(new Person()
            {
                PersonID = Guid.Parse("AC7E45A7-FC1C-4F41-B5A4-EE35E4329115"),
                PersonName = "Lèi",
                Email = "rludron0@china.com.cn",
                Address = "59 Bartillon Circle",
                DateOfBirth = DateTime.Parse("2006-12-14"),
                Gender = "Male",
                ReceiveNewsLetters = false,
                CountryID = Guid.Parse("7313A3DA-9B2C-47CD-8BFA-7CEBFE9055D1")
            });
            _people.Add(new Person()
            {
                PersonID = Guid.Parse("411A11C5-29DA-4819-A300-03F305F02129"),
                PersonName = "Lyséa",
                Email = "mjaulme1@ehow.com",
                Address = "1407 Dorton Drive",
                DateOfBirth = DateTime.Parse("1994-01-26"),
                Gender = "Female",
                ReceiveNewsLetters = false,
                CountryID = Guid.Parse("B6265718-EEF6-4F43-939F-EB5241E5AABC")
            });
            _people.Add(new Person()
            {
                PersonID = Guid.Parse("CF3BEC35-1F06-475F-9F52-CE97AE01069A"),
                PersonName = "Kallisté",
                Email = "crippon2@eepurl.com",
                Address = "5 New Castle Circle",
                DateOfBirth = DateTime.Parse("1979-12-24"),
                Gender = "Female",
                ReceiveNewsLetters = true,
                CountryID = Guid.Parse("6DFFA2C1-7308-4A5B-BDCC-248C3C17C30A")
            });
            _people.Add(new Person()
            {
                PersonID = Guid.Parse("AAC8EE9F-9453-4BDF-B31B-952C6D440DE4"),
                PersonName = "Pénélope",
                Email = "cgreenfield3@printfriendly.com",
                Address = "5 Cambridge Pass",
                DateOfBirth = DateTime.Parse("1981-01-27"),
                Gender = "Female",
                ReceiveNewsLetters = false,
                CountryID = Guid.Parse("6C552E41-5115-49B1-A264-38AC915B1E27")
            });
            _people.Add(new Person()
            {
                PersonID = Guid.Parse("AC7E45A7-FC1C-4F41-B5A4-EE35E4329115"),
                PersonName = "Andréanne",
                Email = "joxberry4@fda.gov",
                Address = "99109 Lyons Avenue",
                DateOfBirth = DateTime.Parse("1986-10-31"),
                Gender = "Male",
                ReceiveNewsLetters = true,
                CountryID = Guid.Parse("2D400B8D-9EA9-4354-A98E-C4ADAFBBBF60")
            });
            _people.Add(new Person()
            {
                PersonID = Guid.Parse("AC7E45A7-FC1C-4F41-B5A4-EE35E4329115"),
                PersonName = "André",
                Email = "mfaro9@latimes.com",
                Address = "439 Lunder Plaza",
                DateOfBirth = DateTime.Parse("1973-12-03"),
                Gender = "Female",
                ReceiveNewsLetters = true,
                CountryID = Guid.Parse("D094E4FA-7AEC-4FCF-A488-AE42A9263814")
            });
            _people.Add(new Person()
            {
                PersonID = Guid.Parse("1F665CA6-2E4D-471E-B4D2-44867D0937A8"),
                PersonName = "Yáo",
                Email = "sscoon5@sakura.ne.jp",
                Address = "3 Acker Plaza",
                DateOfBirth = DateTime.Parse("1981-03-23"),
                Gender = "Male",
                ReceiveNewsLetters = true,
                CountryID = Guid.Parse("D094E4FA-7AEC-4FCF-A488-AE42A9263814")
            });
        }
    }
    private PersonResponse ConvertPerson(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();

        personResponse.Country = _countries.GetCountryByCountryID(person.CountryID)?.CountryName;

        return personResponse;
    }

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest == null)
            throw new ArgumentNullException(nameof(personAddRequest));

        ValidationHelper.ValidateModel(personAddRequest);

        if (string.IsNullOrEmpty(personAddRequest.PersonName))
            throw new ArgumentException(nameof(personAddRequest.PersonName));

        Person p = personAddRequest.ToPerson();

        p.PersonID = Guid.NewGuid();
        _people.Add(p);

        return ConvertPerson(p);
    }

    public List<PersonResponse> GetAllPerson()
    {
        return _people.Select((p) => p.ToPersonResponse()).ToList();
    }

    public PersonResponse? GetPersonByPersonID(Guid? personID)
    {
        if (personID == null)
            throw new ArgumentNullException(nameof(personID));

        return _people.FirstOrDefault((p) => p.PersonID == personID)?.ToPersonResponse();
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetAllPerson();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.PersonName) ?
                temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Person.Email):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Email) ?
                temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;


            case nameof(Person.DateOfBirth):
                matchingPersons = allPersons.Where(temp =>
                (temp.DateOfBirth != null) ?
                temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(Person.Gender):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Gender) ?
                temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            case nameof(Person.CountryID):
                matchingPersons = allPersons.Where(temp =>
                (temp.CountryID is not null) ?
                temp.CountryID.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                break;

            case nameof(Person.Address):
                matchingPersons = allPersons.Where(temp =>
                (!string.IsNullOrEmpty(temp.Address) ?
                temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                break;

            default: matchingPersons = allPersons; break;
        }
        return matchingPersons;
    }

    public List<PersonResponse> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy))
            return allPersons;

        List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
        {
            (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

            (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

            (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

            (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

            _ => allPersons
        };

        return sortedPersons;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(personUpdateRequest));

        ValidationHelper.ValidateModel(personUpdateRequest);

        Person? matchingPerson = _people.FirstOrDefault((p) => p.PersonID == personUpdateRequest.PersonID);

        if (matchingPerson == null)
            throw new ArgumentException("Given person id doen't exists");

        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.Address = personUpdateRequest.Address;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryID = personUpdateRequest.CountryID;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        return matchingPerson.ToPersonResponse();
    }

    public bool DeletePerson(Guid? personID)
    {
        if (personID == null)
            throw new ArgumentNullException(nameof(personID));

        Person? person = _people.FirstOrDefault((p) => p.PersonID == personID);

        if (person == null)
            return false;

        _people.RemoveAll((temp) => temp.PersonID == person.PersonID);

        return true;
    }
}