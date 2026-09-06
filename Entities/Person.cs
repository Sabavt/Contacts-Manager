using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Domain Model for Person
/// </summary>
public class Person
{
    [Key]
    public Guid PersonID { get; set; }

    [StringLength(40)]
    public string? PersonName { get; set; } 

    [StringLength(40)] 
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; } 

    [StringLength(8)] 
    public string? Gender { get; set; } = null!; 
    public Guid? CountryID { get; set; }
    [StringLength(100)]
    public string? Address { get; set; }
    public bool? ReceiveNewsLetters { get; set; }
}
