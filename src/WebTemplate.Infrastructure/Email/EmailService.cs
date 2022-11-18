using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using WebTemplate.Application.Email;

namespace WebTemplate.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> options)
    {
        this._emailOptions = options.Value;
    }

    public async Task SendEmail(string subject, string body, string toAddress, string? toName = null)
    {
        var host = _emailOptions.Host;
        var port = _emailOptions.Port;
        var userName = _emailOptions.UserName;
        var email = _emailOptions.Email;
        var password = _emailOptions.Password; ;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(userName, email));
        message.To.Add(new MailboxAddress(toName ?? toAddress.Split('@').First(), toAddress));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, true).ConfigureAwait(false);
        await client.AuthenticateAsync(email, password).ConfigureAwait(false);
        await client.SendAsync(message).ConfigureAwait(false);
    }
}
