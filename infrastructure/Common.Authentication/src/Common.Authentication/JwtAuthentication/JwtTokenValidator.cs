using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Common.Authentication.JwtAuthentication;

public class JwtTokenValidator
{
    public ClaimsPrincipal ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(JwtSecurityKey.Create);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            // Set clock skew to zero so tokens expire exactly at token expiration time
            ClockSkew = TimeSpan.Zero
        };
        ClaimsPrincipal? principal = null;
        try
        {
            SecurityToken validatedToken;

            principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Invalid token", ex);
        }
        return principal;
    }
}
