using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class PersonsGetterServiceWithCompactExcel : IPersonsGetterService
{
    private readonly IPersonsGetterService _personsGetterService;

    public PersonsGetterServiceWithCompactExcel(IPersonsGetterService personsGetterService)
    {
        _personsGetterService = personsGetterService;
    }

    public Task<List<PersonResponse>> GetAllPerson() => _personsGetterService.GetAllPerson();

    public Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString) => _personsGetterService.GetFilteredPersons(searchBy, searchString);

    public Task<PersonResponse?> GetPersonByPersonID(Guid? personID) => _personsGetterService.GetPersonByPersonID(personID);

    public Task<MemoryStream> GetPersonsCSV() => _personsGetterService.GetPersonsCSV();

    public async Task<MemoryStream> GetPersonsExcel()
    {
        MemoryStream memoryStream = new MemoryStream();
        using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
        {
            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
            workSheet.Cells["A1"].Value = "Person Name";
            workSheet.Cells["B1"].Value = "Email"; 

            using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
            {
                headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                headerCells.Style.Font.Bold = true;
            }
            int row = 3;
            List<PersonResponse> persons = await GetAllPerson();
            foreach (PersonResponse person in persons)
            {
                workSheet.Cells[row, 1].Value = person.PersonName;
                workSheet.Cells[row, 2].Value = person.Email; 

                row++;
            }

            workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

            await excelPackage.SaveAsync();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
}
