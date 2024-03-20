#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Common.Authentication.Attributes;

public class RequiredNonEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string str && string.IsNullOrWhiteSpace(str))
        {
            return new ValidationResult("The field is required.");
        }

        return ValidationResult.Success;
    }
}
