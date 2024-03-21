using Common.DbEventStore;

namespace Authentication.Entities;

public class Role : IEntity
{
    public Guid Id { get; set; }
    public string? name { get; set; }
    public List<Guid> userIds { get; set; } = new List<Guid>();
}
