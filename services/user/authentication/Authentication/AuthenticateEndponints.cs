using System.Diagnostics.Metrics;
using Common.Authentication.JwtAuthentication;
using Common.Authentication.Models;

namespace Authentication.Service.Endpoints;

public static class AuthenticateEndponints
{
    private static ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("MyLogger");
    private static string? _ipAddress;
    private static readonly Counter<int> authenticationRequestCounter = new Meter("play.authentication.service").CreateCounter<int>("authentication_request");
    // public static void Initialize(ILoggerFactory loggerFactory)
    // {
    //     _logger = loggerFactory.CreateLogger("AuthenticateEndponints") ?? throw new ArgumentNullException(nameof(_logger));
    // }
    public static void SetIpAddress(string ipAddress)
    {
        _ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(_ipAddress));
    }

    public static RouteGroupBuilder MapAuthenticateEndpoints(this IEndpointRouteBuilder routes)
    {
        //var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        var group = routes.MapGroup("/authenticate");
        _logger.LogInformation(6, "Mapping /authenticate endpoints started at ...");

        group.MapPost("/", async (AuthenticationRequest authenticationRequest, JwtTokenHandler jwtTokenHandler, HttpContext context) =>
        {
            var (remoteAddress, localAddress) = context.GetIpAddresses();

            var authenticationResponse = await jwtTokenHandler.GenerateJwtToken(authenticationRequest);

            _logger.LogInformation(7, "Authentication request received: {Name} {Remote Address} {Local Address}", authenticationRequest.UserName, remoteAddress, localAddress);
            if (authenticationResponse == null)
            {
                return Results.Unauthorized();
            }

            authenticationRequestCounter.Add(1);
            return Results.Ok(authenticationResponse);
        });

        return group;
    }
}
