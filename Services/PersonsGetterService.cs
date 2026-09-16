using Entities;
using ServiceContracts;
using ServiceContracts.DTO; 
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;
using Exceptions;

namespace Services;

public class PersonsGetterService : IPersonsGetterService
{  
    private readonly IPersonsRepository _personsRepository;
    private readonly ILogger<PersonsGetterService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonsGetterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
    {  
        _diagnosticContext = diagnosticContext;
        _logger = logger;
        _personsRepository = personsRepository;
    } 

    public async Task<List<PersonResponse>> GetAllPerson()
    {
        _logger.LogInformation("GetAllPerson method of PersonsService");

        var persons_from_rep = await _personsRepository.GetAllPersons();

        return persons_from_rep.Select(temp =>  temp.ToPersonResponse()).ToList();
    }

    public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
    {
        if (personID == null)
            throw new InvalidPersonIdException(nameof(personID));

        var person = await _personsRepository.GetPersonByPersonID(personID.Value);

        if(person == null)
            return null;

        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
    {
        _logger.LogInformation("GetFilteredPersons method of PersonsService");

        if(string.IsNullOrEmpty(searchString))
        {
            searchString = "a";
        }

        List<Person>? persons = null;
        using (Operation.Time ("Time for filtering persons"))
        { 
            persons = searchBy switch
            {
                nameof(PersonResponse.PersonName) => await _personsRepository
                .GetFilteredPersons(temp =>
                   temp.PersonName!.Contains(searchString)
                     ),

                nameof(PersonResponse.Email) => await _personsRepository
                .GetFilteredPersons(temp =>
                   temp.Email!.Contains(searchString)
                     ),

                nameof(PersonResponse.DateOfBirth) => await _personsRepository
                .GetFilteredPersons(temp =>
                   temp.DateOfBirth!.Value.ToString("dd MMMM yyyy")
                   .Contains(searchString)
                   ),

                nameof(PersonResponse.Gender) => await _personsRepository
                .GetFilteredPersons(temp =>
                    temp.Gender!.Equals(searchString)
                    ),

                nameof(PersonResponse.Country) => await _personsRepository
                .GetFilteredPersons(temp =>
                     temp.Country!.ToString()!
                     .Contains(searchString)
                     ),

                nameof(PersonResponse.Address) => await _personsRepository
                .GetFilteredPersons(temp =>
                     temp.Address!.Contains(searchString)
                     ),

                _ => await _personsRepository.GetAllPersons()
            };
        }
        _diagnosticContext.Set("Persons", persons);
        return persons.Select(temp => temp.ToPersonResponse()).ToList();
    } 

    public async Task<MemoryStream> GetPersonsCSV(List<PersonResponse> persons)
    {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);

        CsvConfiguration config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
        CsvWriter csvWriter = new CsvWriter(writer,config, leaveOpen: true);

        csvWriter.WriteField(nameof(PersonResponse.PersonName));
        csvWriter.WriteField(nameof(PersonResponse.Age));
        csvWriter.WriteField(nameof(PersonResponse.Gender));
        csvWriter.WriteField(nameof(PersonResponse.Email));
        csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
        csvWriter.WriteField(nameof(PersonResponse.Address));

        csvWriter.NextRecord(); 
        
        foreach(PersonResponse person in persons)
        {
            csvWriter.WriteField(person.PersonName);
            csvWriter.WriteField(person.Age);
            csvWriter.WriteField(person.Gender);
            csvWriter.WriteField(person.Email);
            csvWriter.WriteField(person.DateOfBirth?.ToString("yyyy-MM-dd"));
            csvWriter.WriteField(person.Address);

            csvWriter.NextRecord();
            csvWriter.Flush();
        }
         
        stream.Position = 0;
        return stream;
    }

    public async Task<MemoryStream> GetPersonsExcel(List<PersonResponse> persons)
    {
        MemoryStream memoryStream = new MemoryStream();
        using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
        {
            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
            workSheet.Cells["A1"].Value = "Person Name";
            workSheet.Cells["B1"].Value = "Email";
            workSheet.Cells["C1"].Value = "Date of Birth";
            workSheet.Cells["D1"].Value = "Age";
            workSheet.Cells["E1"].Value = "Gender";
            workSheet.Cells["F1"].Value = "Country";
            workSheet.Cells["G1"].Value = "Address";
            workSheet.Cells["H1"].Value = "Receive News Letters";

            using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
            {
                headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                headerCells.Style.Font.Bold = true;
            }
            int row = 2; 

            foreach (PersonResponse person in persons)
            {
                workSheet.Cells[row, 1].Value = person.PersonName;
                workSheet.Cells[row, 2].Value = person.Email;
                if (person.DateOfBirth.HasValue)
                    workSheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
                workSheet.Cells[row, 4].Value = person.Age;
                workSheet.Cells[row, 5].Value = person.Gender;
                workSheet.Cells[row, 6].Value = person.Country;
                workSheet.Cells[row, 7].Value = person.Address;
                workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

                row++;
            }

            workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

            await excelPackage.SaveAsync();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
} 