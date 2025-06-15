using FutFut.Common;

namespace FutFut.Profile.Service.Entities;

public class FriendShipEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid FriendAId { get; set; }
    public Guid FriendBId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public ProfileEntity? FriendA { get; set; } 
    public ProfileEntity? FriendB { get; set; } 
}