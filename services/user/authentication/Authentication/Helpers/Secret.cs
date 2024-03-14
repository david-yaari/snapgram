using Serilog;

namespace Play.Authentication.Service.Helpers;

public class Secret
{
    string GetSecret(string key, IHostEnvironment environment)
    {
        string? secret = null;

        try
        {
            if (environment.IsDevelopment())
            {
                // In development, read the secret from an environment variable
                secret = Environment.GetEnvironmentVariable(key);
            }
            else
            {
                // In production, read the secret from the Kubernetes secret
                string secretPath = $"/etc/secret/{key.ToLower()}";
                if (File.Exists(secretPath))
                {
                    secret = File.ReadAllText(secretPath);
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            Log.Error("Error reading secret {key}: {ex.Message}", key, ex.Message);
        }

        if (string.IsNullOrEmpty(secret))
        {
            // Log the missing secret
            Log.Error("Secret {key} is missing. Please set the {key} environment variable in development or use a Kubernetes secret in production.", key);

            // Consider whether your application can continue running without the secret
            // If not, you might want to throw an exception or exit here
            throw new Exception($"Secret {key} is missing.");
        }

        return secret;
    }
}
