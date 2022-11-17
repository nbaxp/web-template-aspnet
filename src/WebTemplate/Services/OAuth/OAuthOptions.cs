namespace WebTemplate.Services.OAuth;

public class OAuthOptions
{
    public const string Position = "OAuth";

    public List<OAuthProviderOptions> Providers { get; set; } = new List<OAuthProviderOptions>();
}
