using MongoDB.Bson;

namespace Common.DbEventStore.Tests;

public class User : IEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    // other properties...
}
