using WebTemplate.Application.Shared;

namespace WebTemplate.Application.Entities;

public class RolePermission : BaseEntity, IAssociation
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}
