using FutFut.Notify.Contracts;
using FutFut.Notify.Service.Data.Enums;

namespace FutFut.Notify.Service;

public record SendEmailDto(
    string Address,
    string Subject,
    string Body
);

public record RegisterDeviceDto(
    Guid UserId,
    string Token
);

public record NotificationDto(
    Guid UserId,
    string Payload,
    string Title,
    string Content,
    bool IsRead,
    string Type
);
