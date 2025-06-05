using FutFut.Notify.Contracts;
using FutFut.Notify.Service.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FutFut.Notify.Service.Consumers;

public class SendEmailConsumer(IOptions<EmailSettings> emailOptions): IConsumer<SendEmail>
{
    private readonly EmailSettings _emailSettings = emailOptions.Value;
    
    public async Task Consume(ConsumeContext<SendEmail> context)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
        mimeMessage.To.Add(MailboxAddress.Parse(context.Message.Address));
        mimeMessage.Subject = context.Message.Subject;

        if (context.Message.IsBodyHtml)
        {
            mimeMessage.Body = new TextPart("html") { Text = context.Message.Body };
        }
        else
        {
            mimeMessage.Body = new TextPart("plain") { Text = context.Message.Body };
        }
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
        await smtp.SendAsync(mimeMessage);
        await smtp.DisconnectAsync(true);
    }
}