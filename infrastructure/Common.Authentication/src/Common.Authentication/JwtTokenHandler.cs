using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Common.Authentication.Models;
using Common.Authentication.Repositories;

namespace Common.Authentication.JwtAuthentication;

public class JwtTokenHandler
{
    public static string JwtSecurityKey
    {
        get
        {
            string key = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY") ?? string.Empty;
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("JWT_SECURITY_KEY cannot be null or whitespace.", nameof(JwtSecurityKey));
            }
            return key;
        }
    }
    private readonly UserRepository _userRepository;
    private readonly ILogger<JwtTokenHandler> _logger;
    private const int JWT_TOKEN_EXPIRY_IN_MINUTES = 20;

    public JwtTokenHandler(
        UserRepository repo,
        ILogger<JwtTokenHandler> logger
    )
    {
        _userRepository = new UserRepository();
        _logger = logger;
    }

    public async Task<AuthenticationResponse?> GenerateJwtToken(AuthenticationRequest authenticationRequest)
    {

        if (string.IsNullOrWhiteSpace(authenticationRequest.UserName))
        {
            throw new ArgumentException("Username cannot be null or whitespace.", nameof(authenticationRequest.UserName));
        }
        if (string.IsNullOrWhiteSpace(authenticationRequest.Password))
        {
            throw new ArgumentException("Password cannot be null or whitespace.", nameof(authenticationRequest.Password));
        }

        UserAccount? userAccount = await _userRepository.GetUserAsync(authenticationRequest.UserName, authenticationRequest.Password);

        foreach (var property in typeof(UserAccount).GetProperties())
        {
            if (property.PropertyType == typeof(string))
            {
                string value = (string?)property.GetValue(userAccount) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"{property.Name} cannot be null or whitespace.", property.Name);
                }
            }
        }


        var tokenExpiryTime = DateTime.Now.AddMinutes(JWT_TOKEN_EXPIRY_IN_MINUTES);
        var tokenKey = Encoding.ASCII.GetBytes(JwtSecurityKey);
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim("Role", userAccount!.Role!)
            });

        var sigingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTime,
            SigningCredentials = sigingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return new AuthenticationResponse
        {
            UserName = userAccount.UserName,
            JwtToken = jwtToken,
            JwtTokenExpiryTime = (int)tokenExpiryTime.Subtract(DateTime.Now).TotalSeconds
        };
    }
}
