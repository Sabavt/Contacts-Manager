using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ApplicationDbContext _dbContext;

    public CountriesService(ApplicationDbContext personsDbContext)
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
     
    public async Task<int> UploadCountriesFromExcel(IFormFile fromFile)
    {
        using MemoryStream memoryStream = new MemoryStream();
        await fromFile.CopyToAsync(memoryStream);

        int countriesAddedCount = 0;

        using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
        {
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Countries"];
            int rowCount = excelWorksheet.Dimension.Rows;

            for (int i = 2; i <= rowCount; i++)
            {
                var countryName = excelWorksheet.Cells[i, 1].Value?.ToString()?.Trim();

                if (_dbContext.Countries.Any(temp => temp.CountryName == countryName))
                    continue;
                
                _dbContext.Countries.Add(new Entities.Country() { CountryID = Guid.NewGuid(), CountryName = countryName });
                await _dbContext.SaveChangesAsync();

                countriesAddedCount++;
            }
        }
        return countriesAddedCount;
    }
}
