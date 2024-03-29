﻿using Common.DbEventStore;

namespace Authentication.Entities;

public class User : IEntity
{
    public Guid tenantId { get; set; }
    public Guid id { get; set; }
    public string? userName { get; set; }
    public string? hashedPassword { get; set; }
    public string? salt { get; set; }
    public string? email { get; set; }
    public List<Guid> roleIds { get; set; } = new List<Guid>();
}
