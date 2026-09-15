using Entities;
using ServiceContracts;
using ServiceContracts.DTO; 
using Services.Helpers;  
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog; 

namespace Services;

public class PersonsAdderService : IPersonsAdderService
{  
    private readonly IPersonsRepository _personsRepository;
    private readonly ILogger<PersonsGetterService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonsAdderService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
    {  
        _diagnosticContext = diagnosticContext;
        _logger = logger;
        _personsRepository = personsRepository;
    }
     
    public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest == null)
            throw new ArgumentNullException(nameof(personAddRequest));

        ValidationHelper.ValidateModel(personAddRequest);

        if (string.IsNullOrEmpty(personAddRequest.PersonName))
            throw new ArgumentException(nameof(personAddRequest.PersonName));

        Person person_to_add = personAddRequest.ToPerson();

        person_to_add.PersonID = Guid.NewGuid();
        await _personsRepository.AddPerson(person_to_add);

        return person_to_add.ToPersonResponse();
    } 
} 