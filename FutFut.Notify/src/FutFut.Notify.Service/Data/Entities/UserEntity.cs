using FutFut.Common;

namespace FutFut.Notify.Service.Data.Entities;

public class UserEntity : IEntity
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    
    public List<DeviceEntity> Devices { get; set; } =  new ();
    public List<NotificationEntity> Notifications { get; set; } =  new ();
}