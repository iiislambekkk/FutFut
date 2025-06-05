using FutFut.Notify.Service.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FutFut.Notify.Service.Controllers;

[ApiController]
[Route("email")]
public class SendEmailController(IOptions<EmailSettings> emailOptions) : ControllerBase
{
    private readonly EmailSettings _emailSettings = emailOptions.Value;
    
    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailDto email)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
        mimeMessage.To.Add(MailboxAddress.Parse(email.Address));
        mimeMessage.Subject = email.Subject;
        mimeMessage.Body = new TextPart("html") { Text = email.Body };
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
        await smtp.SendAsync(mimeMessage);
        await smtp.DisconnectAsync(true);
        
        return Ok();
    }
    
}