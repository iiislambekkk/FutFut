using FutFut.Common;

namespace FutFut.Profile.Service.Entities;

public class PlayedHistoryEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public Guid SongId  { get; set; }
    public DateTimeOffset PlayedDate { get; set; } = DateTimeOffset.UtcNow;
    public string Device { get; set; } = String.Empty;
}