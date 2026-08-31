namespace Entities;

/// <summary>
/// Domain Model for Person
/// </summary>
public class Person
{
    public Guid PersonID { get; set; } 
    public string? PersonName { get; set; } 
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;
    public Guid? CountryID { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
}
