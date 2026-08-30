using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly List<Person> _people;
    private readonly ICountriesService _countries;

    public PersonsService()
    {
        _people = new List<Person>();
        _countries = new CountriesService();
    }
    private PersonResponse ConvertPerson(Person person)
    {
        PersonResponse personResponse = person.ToPersonResponse();

        personResponse.Country = _countries.GetCountryByCountryID(person.CountryID)?.CountryName;

        return personResponse;
    }

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        if(personAddRequest == null)
            throw new ArgumentNullException(nameof(personAddRequest));

        if(string.IsNullOrEmpty(personAddRequest.PersonName))
            throw new ArgumentException(nameof(personAddRequest.PersonName));

        Person p = personAddRequest.ToPerson();
       
        p.PersonID = Guid.NewGuid(); 
        _people.Add(p);
         
        return  ConvertPerson(p); 
    }

    public List<PersonResponse> GetAllPerson()
    {
        throw new NotImplementedException();
    }
}
