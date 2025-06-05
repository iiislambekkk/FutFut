using FutFut.Common;

namespace FutFut.Profile.Service.Entities;

public class DescriptionPhotos : IEntity
{
    public Guid Id { get; set; }
    public string Path { get; set; }
}