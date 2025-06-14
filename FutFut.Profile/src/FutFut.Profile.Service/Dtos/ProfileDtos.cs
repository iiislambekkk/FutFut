using System.ComponentModel.DataAnnotations;
using FutFut.Profile.Service.Entities;

namespace FutFut.Profile.Service.Dtos;

public record ProfileDto(Guid Id,
    string DisplayName,
    string Avatar,
    List<AboutPhotoEntity> AboutPhotos,
    List<PlayedHistoryEntity> PlayedHistory,
    List<FriendShipEntity> FriendShips
);

public record CreateProfileDto([Required] string DisplayName, string Avatar);
public record UpdateProfileDto([Required] Guid Id, string? DisplayName, string? Avatar, string? About);
