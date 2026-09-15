using Entities; 
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesGetterService : ICountriesGetterService
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesGetterService(ICountriesRepository countriesRepository)
    {
        _countriesRepository = countriesRepository; 
    } 

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        var countries = await _countriesRepository.GetAllCountries();
        return countries.Select(c => c.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
    {
        if (countryID == null)
            throw new ArgumentNullException(nameof(countryID));

        Country? country_response_from_countries_table = await _countriesRepository.GetCountryByCountryID(countryID.Value);

        if (country_response_from_countries_table == null)
            throw new ArgumentException(nameof(countryID));

        return country_response_from_countries_table.ToCountryResponse();
    } 
}
