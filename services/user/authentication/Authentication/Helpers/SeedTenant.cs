using Authentication.Entities;
using Common.DbEventStore;

namespace Authentication.Helpers;

public class Seed
{
    private readonly IRepository<Tenant> _tenantsRepository;
    private readonly IRepository<Role> _rolesRepository;
    private readonly IRepository<User> _usersRepository;


    public Seed(IRepository<Tenant> tenantRepository, IRepository<Role> rolesRepository, IRepository<User> usersRepository)
    {
        _tenantsRepository = tenantRepository;
        _rolesRepository = rolesRepository;
        _usersRepository = usersRepository;
    }

    public void Tenants()
    {
        var defaultTenant = new Tenant
        {
            tenantId = Guid.NewGuid(),
            id = Guid.NewGuid(),
            name = "Default",
            domain = "default"
        };

        _tenantsRepository.CreateAsync(defaultTenant);
    }

    public void Roles(Guid tenantId)
    {
        var defaultRoles = new List<Role>
    {
        new Role
        {
            tenantId = tenantId,
            id = Guid.NewGuid(),
            name = "Admin"
        },
        new Role
        {
            tenantId = tenantId,
            id = Guid.NewGuid(),
            name = "User"
        }
    };
    }
    public void Users(Guid tenantId, Guid adminRoleId)
    {
        var password = "adminadmin";
        var salt = PasswordHelper.GenerateSalt();
        var hash = PasswordHelper.HashPassword(password, salt);

        var defaultUser = new User
        {
            tenantId = tenantId,
            id = Guid.NewGuid(),
            userName = "admin",
            hashedPassword = hash,
            salt = salt,
            email = "admin@my-company.com",
            roleIds = new List<Guid> { adminRoleId }
        };
        _usersRepository.CreateAsync(defaultUser);
    }
}