namespace Common.Authentication.Models;

public class AuthenticationResponse
{
    public string? UserId { get; set; }
    public string? JwtToken { get; set; }
    public int JwtTokenExpiryTime { get; set; }
    public string? RefreshToken { get; set; }
}
