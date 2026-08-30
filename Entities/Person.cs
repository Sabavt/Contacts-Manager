using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Domain Model for Person
/// </summary>
public class Person
{
    public Guid PersonID { get; set; }

    [Required(ErrorMessage = "Name can't be blank")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "Email can't be blank")] 
    [EmailAddress(ErrorMessage = "Email should be in proper format")]
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryID { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
}
