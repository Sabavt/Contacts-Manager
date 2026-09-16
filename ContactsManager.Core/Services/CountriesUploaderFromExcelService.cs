using Microsoft.AspNetCore.Http; 
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts; 

namespace Services;

public class CountriesUploaderFromExcelService : ICountriesUploaderFromExcelService
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesUploaderFromExcelService(ICountriesRepository countriesRepository)
    {
        _countriesRepository = countriesRepository; 
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
