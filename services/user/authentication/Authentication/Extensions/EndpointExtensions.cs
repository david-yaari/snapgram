using Authentication.Endpoints.Authentication;
using Authentication.Entities;
using Authentication.Helpers;
using Common.DbEventStore;

namespace Authentication.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapAuthenticateEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/authenticate");

        // Get the UserRepository and ILogger<LoginEndpoint> services
        var userAuthenticationService = routes.ServiceProvider.GetRequiredService<UserAuthenticationService>();

        var userRepository = routes.ServiceProvider.GetRequiredService<IRepository<User>>();
        var logger = routes.ServiceProvider.GetRequiredService<ILogger<LoginEndpoint>>();


        var loginEndpoint = new LoginEndpoint(userAuthenticationService, logger);
        loginEndpoint.MapLoginEndpoint(group);
        // Do the same for your other endpoints...

        return routes;
    }
}
