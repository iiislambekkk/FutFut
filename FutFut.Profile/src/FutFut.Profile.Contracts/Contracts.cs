namespace FutFut.Profile.Contracts;

public record ProfileCreated(
    Guid ProfileId,
    string DisplayName
);