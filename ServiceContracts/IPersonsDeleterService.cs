namespace ServiceContracts;

/// <summary>
/// Represents business logic for deleting Person entity
/// </summary>
public interface IPersonsDeleterService
{  
    /// <summary>
    /// Deletes person based on given personID
    /// </summary>
    /// <param name="personID">PersonID to delete</param>
    /// <returns>Retruns true if deletation is true, otherwise false</returns>
    Task<bool> DeletePerson(Guid? personID); 
}
