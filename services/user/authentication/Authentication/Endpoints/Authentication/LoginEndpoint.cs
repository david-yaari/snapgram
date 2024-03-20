using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Authentication.AuthenticationValidators;
using Authentication.Entities;
using Authentication.Models;
using Authentication.Service;
using Common.Authentication.JwtAuthentication;
using Common.DbEventStore;
using MongoDB.Driver;

namespace Authentication.Endpoints.Authentication;

public class LoginEndpoint
{
    private readonly IRepository<User> _usersRepository;
    private readonly ILogger<LoginEndpoint> _logger;
    public LoginEndpoint(IRepository<User> usersRepository, ILogger<LoginEndpoint> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }
    public void MapLoginEndpoint(IEndpointRouteBuilder group)
    {
        group.MapPost("/login", async context =>
        {
            // Get and Log the IP addresses
            var (remoteAddress, localAddress) = context.GetIpAddresses();

            _logger.LogInformation($"Remote IP: {remoteAddress}, Local IP: {localAddress}");


            // Get the AuthenticationRequest from the request body
            var authenticationRequest = await context.Request.ReadFromJsonAsync<AuthenticationRequest>();

            if (authenticationRequest == null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Bad request" });
                return;
            }

            // Create an instance of ValidateAuthenticationRequest
            var validator = new AuthenticationRequestValidator();

            // Validate the request
            bool isValid = validator.Validate(authenticationRequest);

            if (!isValid)
            {
                throw new ArgumentException("Invalid AuthenticationRequest.", nameof(authenticationRequest));
            }

            Expression<Func<User, bool>> filter = u => u.Email == authenticationRequest.Email;

            var users = await _usersRepository.GetAllAsync(filter);

            if (users == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
                return;
            }

            var user = users.FirstOrDefault();

            JwtTokenRequest jwtTokenRequest = new JwtTokenRequest
            {
                UserId = user.Id,
                RolesId = user.RoleIds
            };

            var authenticationResponse = await JwtTokenHandler.GenerateJwtToken(jwtTokenRequest);

            logger.LogInformation(7, "Authentication request received: {Name} {Remote Address} {Local Address}", authenticationRequest.UserName, remoteAddress, localAddress);
            if (authenticationResponse == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(authenticationResponse);

            authenticationRequestCounter.Add(1);
            return Results.Ok(authenticationResponse);

        });
    }
}
