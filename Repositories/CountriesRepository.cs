using Entities;
using RepositoryContracts;

namespace Repositories;

public class CountriesRepository : ICountriesRepository
{
    private readonly ApplicationDbContext _db;

    public CountriesRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Country> AddCountry(Country country)
    {
        _db.Countries.Add(country);
        await _db.SaveChangesAsync();

        return country;
    }

    public Task<List<Country>> GetAllCountries()
    {
        throw new NotImplementedException();
    }

    public Task<Country?> GetCountryByCountryID(Guid? countryID)
    {
        throw new NotImplementedException();
    }

    public Task<Country?> GetCountryByCountryName(string? countryName)
    {
        throw new NotImplementedException();
    }
}
