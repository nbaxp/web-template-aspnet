namespace WebTemplate.Services.OAuth;

public class OAuthProviderOptions
{
    public string Name { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string? ClientIdName { get; set; }
    public string? ClientSecretName { get; set; }
    public string? UserIdName { get; set; }
    public string CallbackPath { get; set; } = null!;
    public string AuthorizationEndpoint { get; set; } = null!;
    public string TokenEndpoint { get; set; } = null!;
    public string UserInformationEndpoint { get; set; } = null!;
    public string? UserIdentificationEndpoint { get; set; }
}
