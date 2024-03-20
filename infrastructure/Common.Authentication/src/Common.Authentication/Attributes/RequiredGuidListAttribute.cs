using System.ComponentModel.DataAnnotations;

namespace Common.Authentication.Attributes;

public class RequiredGuidListAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || (value is List<Guid> list && !list.Any()))
        {
            return new ValidationResult("The field is required.");
        }

        return ValidationResult.Success;
    }
}
