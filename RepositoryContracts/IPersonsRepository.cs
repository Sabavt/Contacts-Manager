using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts;

/// <summary>
/// Represents a repository for managing Person entities in the data source.
/// </summary>
public interface IPersonsRepository
{
    /// <summary>
    /// Adds a new Person entity to the data source.
    /// </summary>
    /// <param name="person">Person object to add</param>
    /// <returns>Returns the person object after adding it to the table</returns>
    Task<Person> AddPerson(Person person);

    /// <summary>
    /// Returns all persons from the data source as a list of Person objects
    /// </summary>
    /// <returns>List of person objects from table</returns>
    Task<List<Person>> GetAllPersons();

    /// <summary>
    /// Returns a Person object based on the given person ID from the data source
    /// </summary>
    /// <param name="personID">Person ID to search</param>
    /// <returns>Returns matching person from persons table</returns>
    Task<Person?> GetPersonByPersonID(Guid? personID);

    /// <summary>
    /// Returns all person objects based on the given expression
    /// </summary>
    /// <param name="predicate">LINQ expression to check</param>
    /// <returns>All matching persons with given condition</returns>
    Task<Person?> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

    /// <summary>
    /// Deletes a person from the data source based on the given person ID
    /// </summary>
    /// <param name="personID">Person Id to search</param>
    /// <returns>Returns true if deletion was successful, otherwise false</returns>
    Task<bool> DeletePersonByPersonID(Guid? personID);

    /// <summary>
    /// Updates the specified Person entity in the data source based on the given person ID
    /// </summary>
    /// <param name="person">Person object to update</param>
    /// <returns>Returns the updated person object</returns>
    Task<Person> UpdatePerson(Person person);
}
