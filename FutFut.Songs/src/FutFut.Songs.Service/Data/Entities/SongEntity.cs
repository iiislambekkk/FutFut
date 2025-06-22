using FutFut.Common;

namespace FutFut.Songs.Service.Data.Entities;

public class SongEntity : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cover { get; set; }
    public Dictionary<string, string> StreamUrls { get; set; }
    public List<ArtistEntity> ArtistEntities { get; set; }  = new ();
}