using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for adding Country entity
/// </summary>
public interface ICountriesAdderService
{
    /// <summary>
    /// Adds country object to the list of countries
    /// </summary>
    /// <param name="country">Country object to add</param>
    /// <returns>Returns the country object after adding it (including newly generated country id)</returns>
    Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest); 
}
