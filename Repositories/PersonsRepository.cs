using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Repositories;

public class PersonsRepository : IPersonsRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<PersonsRepository> _logger;

    public PersonsRepository(ApplicationDbContext db, ILogger<PersonsRepository> logger)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<Person> AddPerson(Person person)
    {
        await _db.Persons.AddAsync(person);
        await _db.SaveChangesAsync();

        return person;
    }

    public async Task<bool> DeletePersonByPersonID(Guid? personID)
    {
        await _db.Persons.Where(p => p.PersonID == personID).ExecuteDeleteAsync();
        return await _db.SaveChangesAsync() > 0; 
    }

    public async Task<List<Person>> GetAllPersons()
    {
        _logger.LogInformation("GetAllPersons of PersonsRepository");

        return await _db.Persons.Include(p => p.Country).ToListAsync();
    }

    public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
    {
        _logger.LogInformation("GetFilteredPersons of PersonsRepository");

        return await _db.Persons.Include(p => p.Country)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<Person?> GetPersonByPersonID(Guid? personID)
    {
        return await _db.Persons.Include(p => p.Country) 
            .FirstOrDefaultAsync(p => p.PersonID == personID);
    }

    public async Task<Person> UpdatePerson(Person person)
    {
        Person? matchingPerson = await GetPersonByPersonID(person.PersonID);

        if (matchingPerson == null)
            return person;

        matchingPerson.PersonName = person.PersonName;
        matchingPerson.Gender = person.Gender;
        matchingPerson.Address = person.Address;
        matchingPerson.TIN = person.TIN;
        matchingPerson.Country = person.Country;
        matchingPerson.DateOfBirth = person.DateOfBirth;
        matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

        await _db.SaveChangesAsync();

        return matchingPerson;
    }
}
