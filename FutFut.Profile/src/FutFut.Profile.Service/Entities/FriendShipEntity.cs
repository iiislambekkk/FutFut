using FutFut.Common;
using FutFut.Profile.Service.Enums;
using MassTransit.Futures.Contracts;

namespace FutFut.Profile.Service.Entities;

public class FriendShipEntity : IEntity
{
    public Guid Id { get; set; }
    
    public Guid RequestedUserId { get; set; }
    public Guid RespondedUserId { get; set; }
    
    public DateTimeOffset StartedAt { get; set; }
    public FriendshipStatusEnum Status { get; set; }
}