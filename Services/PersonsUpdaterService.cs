using ServiceContracts;
using ServiceContracts.DTO; 
using Services.Helpers;  
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog; 
using Exceptions;

namespace Services;

public class PersonsUpdaterService : IPersonsUpdaterService
{  
    private readonly IPersonsRepository _personsRepository;
    private readonly ILogger<PersonsGetterService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonsUpdaterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
    {  
        _diagnosticContext = diagnosticContext;
        _logger = logger;
        _personsRepository = personsRepository;
    } 

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(personUpdateRequest));

        ValidationHelper.ValidateModel(personUpdateRequest);

        if (personUpdateRequest.PersonID == Guid.Empty)
            throw new InvalidPersonIdException(nameof(personUpdateRequest));

          
        return (await _personsRepository.UpdatePerson(personUpdateRequest.ToPerson())).ToPersonResponse(); 
    } 
} 