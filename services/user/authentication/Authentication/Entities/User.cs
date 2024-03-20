using Common.DbEventStore;

namespace Authentication.Entities;

public class User : IEntity
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public List<Guid> RoleIds { get; set; } = new List<Guid>();
}
