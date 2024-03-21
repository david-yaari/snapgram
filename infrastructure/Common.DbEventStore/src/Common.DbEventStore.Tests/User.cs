using MongoDB.Bson;

namespace Common.DbEventStore.Tests;

public class User : IEntity
{
    public Guid tenantId { get; set; }
    public Guid id { get; set; }
    public string? name { get; set; }
    public string? email { get; set; }
    // other properties...
}
