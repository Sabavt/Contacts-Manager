using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for getting Country entity
/// </summary>
public interface ICountriesGetterService
{ 
    /// <summary>
    /// Returns all countries from the country list
    /// </summary>
    /// <returns>All countries from the list as list of CountryRespons</returns>
    Task<List<CountryResponse>> GetAllCountries();

    /// <summary>
    /// Returns country object based on give country id
    /// </summary>
    /// <param name="countryID">Country id to search</param>
    /// <returns>Matching object</returns>
    Task<CountryResponse?> GetCountryByCountryID(Guid? countryID); 
}
