using Entities;
using Microsoft.EntityFrameworkCore;
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

    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    { 
        if(countryAddRequest == null) 
            throw new ArgumentNullException(nameof(countryAddRequest)); 

        if(countryAddRequest.CountryName == null)
            throw new ArgumentException(nameof(countryAddRequest.CountryName));

        if (await _dbContext.Countries.CountAsync(c => c.CountryName == countryAddRequest.CountryName) > 0)
            throw new ArgumentException("Given country name alredy exists");

        Country country = countryAddRequest.ToCountry();

        _dbContext.Countries.Add(country);
        await _dbContext.SaveChangesAsync();

        return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        return await _dbContext.Countries.Select(c => c.ToCountryResponse()).ToListAsync();
    }

    public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
    {
        if (countryID == null)
            return null;
        Country? country_response_from_countries_table = await _dbContext.Countries.FirstOrDefaultAsync((c) => c.CountryID == countryID);

        if (country_response_from_countries_table == null)
            return null;

        return country_response_from_countries_table.ToCountryResponse();
    }
}
