using FutFut.Common;
using FutFut.Notify.Service.Data.Enums;

namespace FutFut.Notify.Service.Data.Entities;

public class DeviceEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Token { get; set; } = String.Empty;
}