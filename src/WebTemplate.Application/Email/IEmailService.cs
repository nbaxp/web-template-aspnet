namespace WebTemplate.Application.Email;

public interface IEmailService
{
    Task SendEmail(string subject, string body, string toAddress, string? toName = null);
}
