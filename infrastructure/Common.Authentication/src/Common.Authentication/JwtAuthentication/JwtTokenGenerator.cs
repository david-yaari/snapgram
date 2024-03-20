using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Authentication.Models;
using Microsoft.Extensions.Logging;
using Common.Authentication.JwtValidators;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Common.Authentication.JwtAuthentication;

public class JwtTokenGenerator
{
    private readonly ILogger<JwtTokenGenerator> _logger;
    private const int JWT_TOKEN_EXPIRY_IN_MINUTES = 20;
    public JwtTokenGenerator(
    ILogger<JwtTokenGenerator> logger
)
    {
        _logger = logger;
    }
    public Task<AuthenticationResponse?> GenerateJwtToken(JwtTokenRequest jwtTokenRequest)
    {

        // Create an instance of ValidateAuthenticationRequest
        var validator = new JwtTokenRequestValidator();

        // Validate the request
        bool isValid = validator.Validate(jwtTokenRequest);

        if (!isValid)
        {
            throw new ArgumentException("Invalid AuthenticationRequest.", nameof(jwtTokenRequest));
        }

        var tokenExpiryTime = DateTime.Now.AddMinutes(JWT_TOKEN_EXPIRY_IN_MINUTES);
        var tokenKey = Encoding.ASCII.GetBytes(JwtSecurityKey.Create);
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, jwtTokenRequest.UserId.ToString()!),
                new Claim("RoleId", jwtTokenRequest.RolesId!.ToString()!)
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

        return Task.FromResult<AuthenticationResponse?>(new AuthenticationResponse
        {
            UserId = jwtTokenRequest.UserId,
            RolesId = jwtTokenRequest.RolesId,
            JwtToken = jwtToken,
            JwtTokenExpiryTime = (int)tokenExpiryTime.Subtract(DateTime.Now).TotalSeconds,
            RefreshToken = GenerateRefreshToken(32) // Generate a 32-byte refresh token
        });
    }

    private string GenerateRefreshToken(int length)
    {
        var byteArr = new byte[length];
        RandomNumberGenerator.Fill(byteArr);

        var refreshToken = Convert.ToBase64String(byteArr);

        return refreshToken;
    }
}
