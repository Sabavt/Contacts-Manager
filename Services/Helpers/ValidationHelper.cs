using System.ComponentModel.DataAnnotations; 

namespace Services.Helpers;

public static class ValidationHelper
{
    public static void ValidateModel(object obj)
    {
        ValidationContext validation = new ValidationContext(obj);
        List<ValidationResult> results = new List<ValidationResult>(); 
        if(!Validator.TryValidateObject(obj, validation, results, true))
        {
            throw new ArgumentException(results.FirstOrDefault()?.ErrorMessage);
        }
    }
}
