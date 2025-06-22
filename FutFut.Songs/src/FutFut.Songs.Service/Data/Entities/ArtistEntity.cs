using FutFut.Common;

namespace FutFut.Songs.Service.Data.Entities;

public class ArtistEntity : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public List<SongEntity> Songs { get; set; } = new();
}