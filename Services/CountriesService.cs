using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService(bool intitialize = true)
    {
        _countries = new List<Country>();
        if(intitialize)
        {
            _countries.AddRange([
            new Country() { CountryID = Guid.Parse("7313A3DA-9B2C-47CD-8BFA-7CEBFE9055D1"), CountryName = "UK" },
            new Country() { CountryID = Guid.Parse("B6265718-EEF6-4F43-939F-EB5241E5AABC"), CountryName = "USA" },
            new Country() { CountryID = Guid.Parse("6DFFA2C1-7308-4A5B-BDCC-248C3C17C30A"), CountryName = "Canada" },
            new Country() { CountryID = Guid.Parse("6C552E41-5115-49B1-A264-38AC915B1E27"), CountryName = "Germany" },
            new Country() { CountryID = Guid.Parse("2D400B8D-9EA9-4354-A98E-C4ADAFBBBF60"), CountryName = "Georgia" },
            new Country() { CountryID = Guid.Parse("D094E4FA-7AEC-4FCF-A488-AE42A9263814"), CountryName = "Brazil" }
            ]);
        } 
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
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
        return _countries.Select(c => c.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryID(Guid? countryID)
    {
        if (countryID == null)
            return null;

        return _countries.FirstOrDefault((c) => c.CountryID == countryID)?.ToCountryResponse();
    }
}
