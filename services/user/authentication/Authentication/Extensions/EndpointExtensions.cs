using Authentication.Endpoints.Authentication;
using Authentication.Entities;
using Common.Authentication.Repositories;
using Common.DbEventStore;

namespace Authentication.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapAuthenticateEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/authenticate");

        // Get the UserRepository and ILogger<LoginEndpoint> services
        var userRepository = routes.ServiceProvider.GetRequiredService<IRepository<User>>();
        var logger = routes.ServiceProvider.GetRequiredService<ILogger<LoginEndpoint>>();


        var loginEndpoint = new LoginEndpoint(userRepository, logger);
        loginEndpoint.MapLoginEndpoint(group);
        // Do the same for your other endpoints...

        return routes;
    }
}
