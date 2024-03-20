namespace Common.Authentication.JwtAuthentication;

public class JwtSecurityKey
{
    public static string Create
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
}
