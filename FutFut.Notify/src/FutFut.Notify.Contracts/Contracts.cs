namespace FutFut.Notify.Contracts;

public record SendEmail(  
    string Address,
    string Subject,
    string Body,
    bool IsBodyHtml
);