using FutFut.Common;
using FutFut.Profile.Service.Enums;
using MassTransit.Futures.Contracts;

namespace FutFut.Profile.Service.Entities;

public class FriendShipRequestEntity : IEntity
{
    public Guid Id { get; set; }
    
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    
    public DateTimeOffset StartedAt { get; set; }
    public FriendshipStatusEnum Status { get; set; }
    
    public ProfileEntity FromProfile { get; set; }
    public ProfileEntity ToProfile { get; set; }
}