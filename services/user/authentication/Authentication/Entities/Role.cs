using Common.DbEventStore;

namespace Authentication.Entities;

public class Role : IEntity
{
    public Guid tenantId { get; set; }
    public Guid id { get; set; }
    public string? name { get; set; }
    public List<Guid> userIds { get; set; } = new List<Guid>();
}
