using ServiceContracts.DTO; 

namespace ServiceContracts;

/// <summary>
/// Represents business logic for searching Person entity
/// </summary>
public interface IPersonsGetterService
{ 
    /// <summary>
    /// Returns all person
    /// </summary>
    /// <returns>Returns a list of objects of PersonResponse type</returns>
    Task<List<PersonResponse>> GetAllPerson();

    /// <summary>
    /// Returns person object based on given person id
    /// </summary>
    /// <param name="personID">Person id to search</param>
    /// <returns>Returns matching person object</returns>
    Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

    /// <summary>
    /// Returns all person objects that matches with the given search field and search string
    /// </summary>
    /// <param name="searchBy">Seach field to search</param>
    /// <param name="searchString">Search string to search</param>
    /// <returns>Returns all matching persons based on the given search field and search string</returns>
    Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString); 
  
    /// <summary>
    /// Returns persons as CSV
    /// </summary>
    /// <returns>Returns MemoryStream with CSV data</returns>
    Task<MemoryStream> GetPersonsCSV();

    /// <summary>
    /// Returns persons as Excel
    /// </summary>
    /// <returns>Returns MemoryStream with Excel data</returns>
    Task<MemoryStream> GetPersonsExcel();
}
