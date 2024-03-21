namespace Common.DbEventStore
{
    public interface IEntity
    {
        Guid tenantId { get; set; }
        Guid id { get; set; }
    }
}

