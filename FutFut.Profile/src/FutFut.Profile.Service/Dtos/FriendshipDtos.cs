namespace FutFut.Profile.Service.Dtos;

public record FriendShipRequestDto(
    Guid RequestedUserId,
    Guid TargetUserId
);