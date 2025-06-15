namespace FutFut.Identity.Contracts;

public record UserCreated(Guid Id, string Email);
public record UserDeleted(Guid Id);
