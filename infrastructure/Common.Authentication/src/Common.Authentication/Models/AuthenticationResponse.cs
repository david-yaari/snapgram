namespace Common.Authentication.Models;

public class AuthenticationResponse
{
    public string? UserName { get; set; }
    public string? JwtToken { get; set; }
    public int JwtTokenExpiryTime { get; set; }
}
