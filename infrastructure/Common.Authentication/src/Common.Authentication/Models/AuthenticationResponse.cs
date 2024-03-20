namespace Common.Authentication.Models;

public class AuthenticationResponse
{
    public Guid? UserId { get; set; }
    public List<Guid>? RolesId { get; set; }
    public string? JwtToken { get; set; }
    public int JwtTokenExpiryTime { get; set; }
    public string? RefreshToken { get; set; }
}
