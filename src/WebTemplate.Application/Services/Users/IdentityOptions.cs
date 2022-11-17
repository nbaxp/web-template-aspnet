namespace WebTemplate.Application.Services.Users;

public class IdentityOptions
{
    public const string Position = "Identity";
    public bool SupportsUserLockout { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
    public TimeSpan DefaultLockoutTimeSpan { get; set; }
}
