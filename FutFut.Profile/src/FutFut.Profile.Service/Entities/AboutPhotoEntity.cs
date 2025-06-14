using FutFut.Common;

namespace FutFut.Profile.Service.Entities;

public class AboutPhotoEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public string Path { get; set; }
}