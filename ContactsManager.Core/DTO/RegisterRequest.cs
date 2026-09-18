using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations; 

namespace ContactsManager.Core.DTO;

public class RegisterRequest
{
    [Required(ErrorMessage = "Name can't be blank")] 
    public string PersonName { get; set; } = null!;

    [Required(ErrorMessage = "Email can't be blank")] 
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [StringLength(100)]
    [Remote("EmailAlredyExists", "Account", ErrorMessage = "Email alredy exists, please try a different one")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone can't be blank")] 
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Password can't be blank")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password can't be blank")]
    [Compare(nameof(Password), ErrorMessage = "Confirm Password must match with Password")] 
    [DataType(DataType.Password)] 
    public string ConfirmPassword { get; set; } = null!;

    public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
}
