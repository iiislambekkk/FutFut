namespace FutFut.Notify.Service;

public record SendEmailDto(
    string Address,
    string Subject,
    string Body
);