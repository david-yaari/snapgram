using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace Authentication.Service;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> logger = logger;
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceID = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogError(
            exception,
            "Could not process a request on machine {MachineName} with traceID: {TraceId}: {Message}",
            Environment.MachineName,
            traceID,
            exception.Message);

        var (statusCode, title) = MapException(exception);

        await Results.Problem(
            title: title,
            //detail: "Please try again later.",
            statusCode: statusCode,
            extensions: new Dictionary<string, object?> { ["traceId"] = traceID }
            ).ExecuteAsync(httpContext);

        return true;
    }

    private static (int StatusCode, string Title) MapException(Exception exception)
    {
        return exception switch
        {
            // Map known exceptions to status codes
            InvalidOperationException _ or ArgumentException _ => (StatusCodes.Status401Unauthorized, "You are not authorized to access this resource."),
            _ => (StatusCodes.Status500InternalServerError, "An error occurred while processing your request."),
        };
    }
}