using FutFut.Common;

namespace FutFut.Profile.Service.Entities;

public class PlayedHistory : IEntity
{
    public Guid Id { get; set; }
    public Guid SongId  { get; set; }
    public DateTimeOffset PlayedDate { get; set; }
    public string Device { get; set; } = String.Empty;
}