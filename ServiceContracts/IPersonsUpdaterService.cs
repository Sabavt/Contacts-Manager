using ServiceContracts.DTO; 

namespace ServiceContracts;

/// <summary>
/// Represents business logic for updating Person entity
/// </summary>
public interface IPersonsUpdaterService
{ 
    /// <summary>
    /// Updates the specified person details based on the given person ID
    /// </summary>
    /// <param name="personUpdateRequest">Person details to update, including person id</param>
    /// <returns>Return updated PersonResponde object after updation</returns>
    Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest); 
}
