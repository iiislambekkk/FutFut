using System.Net;
using System.Net.Mail;
using FutFut.Identity.Service.Settings;
using FutFut.Notify.Contracts;
using MassTransit;
using Microsoft.Extensions.Options;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}

public class EmailSender(IOptions<EmailSettings> emailSettings, IPublishEndpoint publishEndpoint) : IEmailSender
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Sending email to {email}...");
        await publishEndpoint.Publish<SendEmail>(new(email, subject, message, true));
        
        Console.WriteLine($"Email sent to {email}.");
        Console.ResetColor();
    }
}