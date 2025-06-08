using FutFut.Common;

namespace FutFut.Profile.Service.Entities;

public class SystemWorks : IEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset? TimeOfWork { get; set; }
}