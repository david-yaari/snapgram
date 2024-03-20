// using System.Diagnostics.Metrics;
// using Authentication.AuthenticationValidators;
// using Authentication.Endpoints.Authentication;
// using Authentication.Models;
// using Common.Authentication.JwtAuthentication;

// namespace Authentication.Service.Endpoints;

// public static class AuthenticateEndponints
// {
//     private static ILogger _logger;
//     private static string? _ipAddress;

//     static AuthenticateEndponints()
//     {
//         _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("AuthenticateEndponints");
//     }
//     private static readonly Counter<int> authenticationRequestCounter = new Meter("play.authentication.service").CreateCounter<int>("authentication_request");
//     public static void SetIpAddress(string ipAddress)
//     {
//         _ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(_ipAddress));
//     }

//     public static RouteGroupBuilder MapAuthenticateEndpoints(this IEndpointRouteBuilder routes)
//     {
//         LoginEndpoint.MapLoginEndpoint(routes);
//         // Do the same for your other endpoints...
//         var group = routes.MapGroup("/authenticate");

//         _logger.LogInformation(6, "Mapping /authenticate endpoints started at ...");

//         group.MapPost("/login", async (AuthenticationRequest authenticationRequest, JwtTokenHandler jwtTokenHandler, HttpContext context) =>
//         {
//             // Create an instance of ValidateAuthenticationRequest
//             var validator = new AuthenticationRequestValidator();

//             // Validate the request
//             bool isValid = validator.Validate(authenticationRequest);

//             if (!isValid)
//             {
//                 throw new ArgumentException("Invalid AuthenticationRequest.", nameof(authenticationRequest));
//             }
//             var (remoteAddress, localAddress) = context.GetIpAddresses();



//             var authenticationResponse = await jwtTokenHandler.GenerateJwtToken(authenticationRequest);

//             _logger.LogInformation(7, "Authentication request received: {Name} {Remote Address} {Local Address}", authenticationRequest.UserName, remoteAddress, localAddress);
//             if (authenticationResponse == null)
//             {
//                 return Results.Unauthorized();
//             }

//             authenticationRequestCounter.Add(1);
//             return Results.Ok(authenticationResponse);
//         });

//         return group;
//     }
// }
