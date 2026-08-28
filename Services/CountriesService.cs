using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = new List<Country>();
    }

    public CountryResponse AddCountry(CountryRequest? countryAddRequest)
    { 
        if(countryAddRequest == null) 
            throw new ArgumentNullException(nameof(countryAddRequest)); 

        if(countryAddRequest.CountryName == null)
            throw new ArgumentException(nameof(countryAddRequest.CountryName));

        if (_countries.Where(c => c.CountryName == countryAddRequest.CountryName).Count() > 0)
            throw new ArgumentException("Given country name alredy exists");

        Country country = countryAddRequest.ToCountry();  

        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        throw new NotImplementedException();
    }
}
