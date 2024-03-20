using System.ComponentModel.DataAnnotations;
using Authentication.Models;

namespace Authentication.AuthenticationValidators;

public class AuthenticationRequestValidator
{
    public bool Validate(AuthenticationRequest request)
    {
        var validationContext = new ValidationContext(request);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

        return isValid;
    }
}
