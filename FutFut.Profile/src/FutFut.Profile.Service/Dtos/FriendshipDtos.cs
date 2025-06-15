namespace FutFut.Profile.Service.Dtos;

public record FriendShipRequestDto(
    Guid FromUserId,
    Guid ToUserId
);

public record FriendshipResponseDto(
    bool Accepted,
    Guid FriendshipRequestId
);

public record FriendDto(DateTimeOffset FriendShipStartedAt, ProfileDto Profile);