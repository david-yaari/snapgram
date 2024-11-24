using System.Linq.Expressions;
using Authentication.AuthenticationValidators;
using Authentication.Entities;
using Authentication.Models;
using Common.Authentication.JwtAuthentication;
using Common.Authentication.Models;
using Common.DbEventStore;

namespace Authentication.Helpers;

public class UserAuthenticationService
{
    private readonly IRepository<Tenant> _tenantsRepository;
    private readonly IRepository<Role> _rolesRepository;
    private readonly IRepository<User> _usersRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public UserAuthenticationService(IRepository<Tenant> tenantsRepository, IRepository<User> usersRepository, IRepository<Role> rolesRepository, JwtTokenGenerator jwtTokenGenerator)
    {
        _tenantsRepository = tenantsRepository;
        _usersRepository = usersRepository;
        _rolesRepository = rolesRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public bool ValidateRequest(AuthenticationRequest authenticationRequest)
    {
        var validator = new AuthenticationRequestValidator();
        return validator.Validate(authenticationRequest);
    }

    public async Task<User?> GetUserByEmail(Guid? tenantId, string email, string password)
    {
        var currentTenantId = tenantId ?? Guid.Empty;

        var existingTenants = await _tenantsRepository.GetAllAsync();

        if (!existingTenants.Any())
        {
            var seeder = new Seed(_tenantsRepository, _rolesRepository, _usersRepository);

            seeder.Tenants();

            var firstTenant = await _tenantsRepository.FirstOrDefaultAsync();

            currentTenantId = firstTenant.tenantId;

            seeder.Roles(currentTenantId);
            var adminRole = await _rolesRepository.GetAsync(currentTenantId, r => r.name == "Admin");
            seeder.Users(currentTenantId, adminRole.id);
        }

        // var existingUsers = await _usersRepository.GetAllAsync();
        // if (!existingUsers.Any())
        // {
        //     var salt = PasswordHelper.GenerateSalt();
        //     var hash = PasswordHelper.HashPassword(password, salt);
        //     var defaultUser = new User
        //     {
        //         id = Guid.NewGuid(),
        //         userName = "admin",
        //         hashedPassword = hash,
        //         salt = salt, // Add the salt to the user
        //         email = "admin@my-company.com",
        //         roleIds = new List<Guid> { /* Add any default roles here */ }
        //     };

        //     await _usersRepository.CreateAsync(defaultUser);
        // }

        Expression<Func<User, bool>> filter = u => u.email == email;
        var users = await _usersRepository.GetAllAsync(currentTenantId, filter);
        var user = users.FirstOrDefault();
        if (user != null && user.salt != null)
        {
            string hashedPassword = PasswordHelper.HashPassword(password, user.salt);
            if (user.hashedPassword == hashedPassword)
            {
                return user;
            }
        }

        return null;
    }

    public async Task<AuthenticationResponse?> GenerateJwtToken(User user)
    {
        var jwtTokenRequest = new JwtTokenRequest
        {
            UserId = user.id,
            RolesId = user.roleIds
        };

        return await _jwtTokenGenerator.GenerateJwtToken(jwtTokenRequest);
    }
}
