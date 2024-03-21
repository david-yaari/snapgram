using Common.DbEventStore;

namespace Authentication.Entities;

public class Tenant : IEntity
{
    public Guid tenantId { get; set; } // Unique identifier for the tenant    
    public Guid id { get; set; } // Unique identifier for the tenant
    public string? name { get; set; } // Name of the tenant
    public string? domain { get; set; } // Domain associated with the tenant (optional)
}
