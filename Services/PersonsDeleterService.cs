using ServiceContracts; 
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog; 
using Exceptions;

namespace Services;

public class PersonsDeleterService : IPersonsDeleterService
{  
    private readonly IPersonsRepository _personsRepository;
    private readonly ILogger<PersonsGetterService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonsDeleterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
    {  
        _diagnosticContext = diagnosticContext;
        _logger = logger;
        _personsRepository = personsRepository;
    } 

    public async Task<bool> DeletePerson(Guid? personID)
    {
        if (personID == null)
            throw new InvalidPersonIdException(nameof(personID));  

        return await _personsRepository.DeletePersonByPersonID(personID.Value);
    } 
} 