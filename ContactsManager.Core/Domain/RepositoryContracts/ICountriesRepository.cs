using Entities;

namespace RepositoryContracts;

/// <summary>
/// Represents the repository contract for Country entity
/// </summary>
public interface ICountriesRepository
{
    /// <summary>
    /// Adds a new Country object to the database
    /// </summary>
    /// <param name="country">Country object to add</param>
    /// <returns>Returns Country object after adding it into database</returns>
    Task<Country> AddCountry(Country country);

    /// <summary>
    /// Returns all Country objects from the database
    /// </summary>
    /// <returns>All countries from the table</returns>
    Task<List<Country>> GetAllCountries();

    /// <summary>
    /// Returns country object based on given country id, othewise it returns null
    /// </summary>
    /// <param name="countryID">CountryID to search</param>
    /// <returns>Matching country or null</returns>
    Task<Country?> GetCountryByCountryID(Guid? countryID);

    /// <summary>
    /// Returns country object based on given country name, otherwise it returns null
    /// </summary>
    /// <param name="countryName">CountryName to search</param>
    /// <returns>Matching country from table or null</returns>
    Task<Country?> GetCountryByCountryName(string? countryName);
}
