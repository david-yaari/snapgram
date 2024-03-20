using Common.DbEventStore;

namespace Authentication.Entities;

public class RefreshToken : IEntity
{
    public Guid Id { get; set; } // Unique ID for the refresh token
    public Guid UserId { get; set; } // ID of the user that the token belongs to
    public string? Token { get; set; } // The refresh token itself
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime AddedDate { get; set; }
}
