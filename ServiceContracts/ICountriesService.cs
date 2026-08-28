using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Country entity
/// </summary>
public interface ICountriesService
{
    /// <summary>
    /// Adds country object to the list of countries
    /// </summary>
    /// <param name="country">Country object to add</param>
    /// <returns>Returns the country object after adding it (including newly generated country id)</returns>
    CountryResponse AddCountry(CountryRequest? countryAddRequest);

    /// <summary>
    /// Returns all countries from the country list
    /// </summary>
    /// <returns>All countries from the list as list of CountryResponse</returns>
    List<CountryResponse> GetAllCountries();
}
