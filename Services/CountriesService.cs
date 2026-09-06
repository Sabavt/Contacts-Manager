using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly PersonsDbContext _dbContext;

    public CountriesService(PersonsDbContext personsDbContext)
    {
        _dbContext = personsDbContext; 
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    { 
        if(countryAddRequest == null) 
            throw new ArgumentNullException(nameof(countryAddRequest)); 

        if(countryAddRequest.CountryName == null)
            throw new ArgumentException(nameof(countryAddRequest.CountryName));

        if (_dbContext.Countries.Where(c => c.CountryName == countryAddRequest.CountryName).Count() > 0)
            throw new ArgumentException("Given country name alredy exists");

        Country country = countryAddRequest.ToCountry();

        _dbContext.Countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _dbContext.Countries.Select(c => c.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryID(Guid? countryID)
    {
        if (countryID == null)
            return null;

        return _dbContext.Countries.FirstOrDefault((c) => c.CountryID == countryID)?.ToCountryResponse();
    }
}
