using Microsoft.AspNetCore.Diagnostics;

namespace Authentication;

public class ErrorEndpoint
{
    public void MapLoginEndpoint(IEndpointRouteBuilder group)
    {
        group.MapGet("/error", new Func<HttpContext, Task>(context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Log the exception...

        return Task.FromResult(Results.Problem("An error occurred while processing your request.", statusCode: StatusCodes.Status500InternalServerError));
    }));
    }
}