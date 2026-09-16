using Microsoft.AspNetCore.Http; 

namespace ServiceContracts;

/// <summary>
/// Represents business logic for xml uploader of Countries entity
/// </summary>
public interface ICountriesUploaderFromExcelService
{ 
     
    /// <summary>
    /// Uploads countries from the given excel file
    /// </summary>
    /// <param name="formFile">Excel file with list of countries</param>
    /// <returns>Returns count of countries added into database</returns>
    Task<int> UploadCountriesFromExcel(IFormFile fromFile);
}
