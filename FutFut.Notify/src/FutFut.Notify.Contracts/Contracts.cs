namespace FutFut.Notify.Contracts;

public record SendEmail(  
    string Address,
    string Subject,
    string Body,
    bool IsBodyHtml
);

public record SendNotification(
    Guid UserId,
    string Title,
    string Content,
    string Payload,
    NotificationType Type
);

