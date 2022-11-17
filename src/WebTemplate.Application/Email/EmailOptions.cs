namespace WebTemplate.Application.Email;

public class EmailOptions
{
    public const string Position = "Email";
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
