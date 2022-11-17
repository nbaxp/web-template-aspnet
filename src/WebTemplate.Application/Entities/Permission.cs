using System.ComponentModel.DataAnnotations;
using WebTemplate.Application.Shared;

namespace WebTemplate.Application.Entities;

[Display(Name = "权限")]
public class Permission : BaseEntity
{
    public string Name { get; set; } = null!;
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
