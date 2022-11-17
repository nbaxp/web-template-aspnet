using System.ComponentModel.DataAnnotations;
using WebTemplate.Application.Shared;

namespace WebTemplate.Application.Entities;

[Display(Name = "角色")]
public class Role : BaseEntity
{
    [Display(Name = "名称")]
    public string Name { get; set; } = null!;

    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
