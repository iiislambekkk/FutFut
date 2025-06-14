using System.ComponentModel.DataAnnotations;
using FutFut.Common;

namespace FutFut.Profile.Service.Entities;

public class ProfileEntity : IEntity
{
    public Guid Id { get; set; }
    
    [MaxLength(64)]
    public string DisplayName { get; set; } = String.Empty;
    
    [MaxLength(1000)]
    public string Avatar { get; set; } = String.Empty;
    public List<PlayedHistoryEntity> PlayedHistory { get; set; } = new List<PlayedHistoryEntity>();
    
    [MaxLength(1000)]
    public string About { get; set; } = String.Empty;
    public List<AboutPhotoEntity> AboutPhotos { get; set; } = new List<AboutPhotoEntity>();
    public List<FriendShipEntity> FriendShips { get; set; } = new List<FriendShipEntity>();
    
    public bool ShowFriends { get; set; } = true;
    public bool ShowPlayingSong { get; set; } = true;
    public bool ShowRecentlyPlayed { get; set; } = true;
    
    public bool IsPrivate { get; set; } = false;
}