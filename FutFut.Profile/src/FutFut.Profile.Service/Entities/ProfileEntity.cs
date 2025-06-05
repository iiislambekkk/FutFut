using FutFut.Common;

namespace FutFut.Profile.Service.Entities;

public class ProfileEntity : IEntity
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = String.Empty;
    public string Avatar { get; set; } = String.Empty;
    public List<DescriptionPhotos> DescriptionPhotos { get; set; } = new List<DescriptionPhotos>();
}