using System.Diagnostics.Metrics;
using Authentication.Entities;
using Authentication.Helpers;
using Authentication.Models;
using Authentication.Service;
using Common.DbEventStore;

namespace Authentication.Endpoints.Authentication;

public class LoginEndpoint
{
    private readonly UserAuthenticationService _userAuthenticationService;

    private readonly ILogger<LoginEndpoint> _logger;
    private static readonly Counter<int> authenticationRequestCounter = new Meter("play.authentication.service").CreateCounter<int>("authentication_request");
    public LoginEndpoint(UserAuthenticationService userAuthenticationService, ILogger<LoginEndpoint> logger)
    {
        _userAuthenticationService = userAuthenticationService;
        _logger = logger;
    }
    public void MapLoginEndpoint(IEndpointRouteBuilder group)
    {
        group.MapPost("/login", (Delegate)HandleLogin);

    }
    public async Task<IResult> HandleLogin(HttpContext context)
    {

        // Get and Log the IP addresses
        var (remoteAddress, localAddress) = context.GetIpAddresses();

        _logger.LogInformation($"Remote IP: {remoteAddress}, Local IP: {localAddress}");


        // Get the AuthenticationRequest from the request body
        var authenticationRequest = await context.Request.ReadFromJsonAsync<AuthenticationRequest>();

        if (authenticationRequest == null || !_userAuthenticationService.ValidateRequest(authenticationRequest))
        {
            _logger.LogWarning(6, "Invalid AuthenticationRequest received: {Remote Address} {Local Address}",
            remoteAddress, localAddress);

            return Results.BadRequest(new { error = "Invalid AuthenticationRequest." });
        }

        var user = await _userAuthenticationService.GetUserByEmail(authenticationRequest.TenantId, authenticationRequest.Email!, authenticationRequest.Password!);
        if (user == null)
        {
            _logger.LogWarning(6, "User not found: {Remote Address} {Local Address} {Email}", remoteAddress, localAddress, authenticationRequest.Email);
            return Results.Unauthorized();
        }

        var authenticationResponse = _userAuthenticationService.GenerateJwtToken(user);
        if (authenticationResponse == null)
        {
            return Results.Unauthorized();
        }

        _logger.LogInformation(7, "Authentication request successful: {Email} {Remote Address} {Local Address}", authenticationRequest.Email, remoteAddress, localAddress);
        // if (authenticationResponse == null)
        // {
        //     return Results.Unauthorized();
        // }
        authenticationRequestCounter.Add(1);

        return Results.Ok(authenticationResponse);

    }
}

