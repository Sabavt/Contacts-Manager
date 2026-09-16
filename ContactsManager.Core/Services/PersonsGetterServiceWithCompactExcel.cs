using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class PersonsGetterServiceWithCompactExcel : IPersonsGetterService
{
    private readonly PersonsGetterService _personsGetterService;

    public PersonsGetterServiceWithCompactExcel(PersonsGetterService personsGetterService)
    {
        _personsGetterService = personsGetterService;
    }

    public async Task<List<PersonResponse>> GetAllPerson() => await _personsGetterService.GetAllPerson();

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString) => await _personsGetterService.GetFilteredPersons(searchBy, searchString);

    public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID) => await _personsGetterService.GetPersonByPersonID(personID);

    public async Task<MemoryStream> GetPersonsCSV(List<PersonResponse> persons) => await _personsGetterService.GetPersonsCSV(persons);

    public async Task<MemoryStream> GetPersonsExcel(List<PersonResponse> persons)
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
