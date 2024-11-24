using System.ComponentModel.DataAnnotations;
using Authentication.Attribute;

namespace Authentication.Models;

public class AuthenticationRequest
{
    [Required]
    public Guid? TenantId { get; set; }

    [RequiredNonEmpty]
    [EmailAddress]
    [MinLength(4)]
    public string? Email { get; set; }

    [RequiredNonEmpty]
    [MinLength(8)]
    public string? Password { get; set; }
}
