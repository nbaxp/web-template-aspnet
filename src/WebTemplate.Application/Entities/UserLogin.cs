using WebTemplate.Application.Shared;

namespace WebTemplate.Application.Entities;

public class UserLogin : BaseEntity
{
    public string LoginProvider { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
