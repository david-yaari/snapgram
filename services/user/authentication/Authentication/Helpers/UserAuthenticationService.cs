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
    private readonly IRepository<User> _usersRepository;
    private readonly IRepository<Role> _rolesRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public UserAuthenticationService(IRepository<User> usersRepository, IRepository<Role> rolesRepository, JwtTokenGenerator jwtTokenGenerator)
    {
        _usersRepository = usersRepository;
        _rolesRepository = rolesRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public bool ValidateRequest(AuthenticationRequest authenticationRequest)
    {
        var validator = new AuthenticationRequestValidator();
        return validator.Validate(authenticationRequest);
    }

    public async Task<User?> GetUserByEmail(string email, string password)
    {
        var existingTenants = await _usersRepository.GetAllAsync();
        var existingRoles = await _rolesRepository.GetAllAsync();
        if (!existingRoles.Any())
        {
            var defaultRoles = new List<Role>
            {
                new Role
                {
                    id = Guid.NewGuid(),
                    name = "Admin"
                },
                new Role
                {
                    id = Guid.NewGuid(),
                    name = "User"
                }
            };

            await _rolesRepository.CreateManyAsync(defaultRoles);
        }
        var existingUsers = await _usersRepository.GetAllAsync();
        if (!existingUsers.Any())
        {
            var salt = PasswordHelper.GenerateSalt();
            var hash = PasswordHelper.HashPassword(password, salt);
            var defaultUser = new User
            {
                id = Guid.NewGuid(),
                userName = "admin",
                hashedPassword = hash,
                salt = salt, // Add the salt to the user
                email = "admin@my-company.com",
                roleIds = new List<Guid> { /* Add any default roles here */ }
            };

            await _usersRepository.CreateAsync(defaultUser);
        }

        Expression<Func<User, bool>> filter = u => u.email == email;
        var users = await _usersRepository.GetAllAsync(filter);
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
