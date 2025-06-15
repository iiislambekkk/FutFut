using System.Runtime.InteropServices.JavaScript;
using FutFut.Common;
using FutFut.Notify.Contracts;
using FutFut.Notify.Service.Data.Enums;

namespace FutFut.Notify.Service.Data.Entities;

public class NotificationEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public NotificationType Type { get; set; }
    
    public string Payload { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsRead { get; set; } = false;
    
    public List<DeviceEntity> Devices { get; set; } =  new (); }