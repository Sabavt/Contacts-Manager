using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesService(ICountriesRepository countriesRepository)
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

                if (await _countriesRepository.GetCountryByCountryName(countryName) is not null)
                    continue; 

                await _countriesRepository.AddCountry(new Entities.Country() { CountryID = Guid.NewGuid(), CountryName = countryName }); 

                countriesAddedCount++;
            }
        }
        return countriesAddedCount;
    }
}
