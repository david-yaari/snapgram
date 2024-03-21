using Authentication.Entities;
using Common.DbEventStore;

namespace Authentication.Helpers;

public class Seed
{
    private readonly IRepository<Tenant> _tenantsRepository;

    public Seed(IRepository<Tenant> tenantRepository)
    {
        _tenantsRepository = tenantRepository;
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

}
