using System.ComponentModel.DataAnnotations;
using Common.Authentication.Models;

namespace Common.Authentication.JwtValidators;

public class JwtTokenRequestValidator
{
    public bool Validate(JwtTokenRequest request)
    {
        var validationContext = new ValidationContext(request);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

        return isValid;
    }
}
