using WebTemplate.Services.OAuth;

namespace WebTemplate.Settings;

public class AppSettings
{
    public List<OAuthOptions> OAuth2 { get; set; } = new List<OAuthOptions>();
}
