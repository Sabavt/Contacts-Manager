using Entities; 
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesAdderService : ICountriesAdderService
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesAdderService(ICountriesRepository countriesRepository)
    {
        _countriesRepository = countriesRepository; 
    }

    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    { 
        if(countryAddRequest is null) 
            throw new ArgumentNullException(nameof(countryAddRequest)); 
        
        if(countryAddRequest.CountryName is null)
            throw new ArgumentException(nameof(countryAddRequest.CountryName));

        if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) is not null)
            throw new ArgumentException("Given country name alredy exists");

        Country country = countryAddRequest.ToCountry();

        await _countriesRepository.AddCountry(country); 

        return country.ToCountryResponse();
    } 
}
