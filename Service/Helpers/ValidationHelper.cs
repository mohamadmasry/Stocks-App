using System.ComponentModel.DataAnnotations;

namespace Service.Helpers;

public class ValidationHelper
{
    public static void ModelValidation(object obj)
    {
        ValidationContext validationContext = new ValidationContext(obj);
        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
        if (!isValid)
        {
            throw new ArgumentException(validationResults[0].ErrorMessage, nameof(obj));
        }
    }
}