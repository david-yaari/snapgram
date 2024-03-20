using System.ComponentModel.DataAnnotations;

namespace Common.Authentication.Attributes;

public class RequiredGuidAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || (value is Guid guid && guid == Guid.Empty))
        {
            return new ValidationResult("The field is required.");
        }

        return ValidationResult.Success;
    }
}
