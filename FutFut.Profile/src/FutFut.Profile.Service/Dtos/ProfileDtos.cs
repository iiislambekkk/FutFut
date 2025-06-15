using System.ComponentModel.DataAnnotations;
using FutFut.Profile.Service.Entities;

namespace FutFut.Profile.Service.Dtos;

public record ProfileDto(
    Guid Id,
    string DisplayName,
    string Avatar,
    List<AboutPhotoEntity> AboutPhotos,
    List<PlayedHistoryEntity> PlayedHistory,
    List<ProfileEntity> Friends,
    bool ShowFriends,
    bool ShowPlayingSong,
    bool ShowRecentlyPlayed,
    bool IsPrivate
);

public record CreateProfileDto([Required] string DisplayName);
public record UpdateProfileDto([Required] Guid Id, string? DisplayName, string? Avatar, string? About);
