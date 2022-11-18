using WebTemplate.Application.Shared;

namespace WebTemplate.Application.Entities.OpenId;

public class OpenIdClient : BaseEntity
{
    public string ClientId { get; set; } = null!;
    public string ClientSSecret { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public string HomeUrl { get; set; } = null!;
    public string CallbackUrl { get; set; } = null!;
}
