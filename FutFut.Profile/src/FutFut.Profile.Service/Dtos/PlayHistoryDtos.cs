namespace FutFut.Profile.Service.Dtos;

public record PlayedHistoryDto(
    Guid Id,
    Guid ProfileId,
    Guid SongId,
    DateTimeOffset PlayedDate,
    string Device
);

public record CreatePlayedHistoryDto(
    Guid ProfileId,
    Guid SongId,
    DateTimeOffset PlayedDate,
    string Device
);

