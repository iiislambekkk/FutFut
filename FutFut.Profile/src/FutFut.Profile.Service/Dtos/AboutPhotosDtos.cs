namespace FutFut.Profile.Service.Dtos;

public record AboutPhotoDto(
    Guid Id,
    string Path
);

public record DeleteAboutPhotoDto(
    Guid AboutPhotoId,
    Guid UserId
);