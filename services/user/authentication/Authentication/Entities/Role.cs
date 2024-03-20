using Common.DbEventStore;

namespace Authentication.Entities;

public class Role : IEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public List<Guid> UserIds { get; set; } = new List<Guid>();
}
