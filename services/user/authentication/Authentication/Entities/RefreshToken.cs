using Common.DbEventStore;

namespace Authentication.Entities;

public class RefreshToken : IEntity
{
    public Guid tenantId { get; set; } // ID of the tenant that the token belongs to
    public Guid id { get; set; } // Unique ID for the refresh token
    public Guid userId { get; set; } // ID of the user that the token belongs to
    public string? token { get; set; } // The refresh token itself
    public DateTime expiryDate { get; set; }
    public bool isRevoked { get; set; }
    public DateTime addedDate { get; set; }
}
