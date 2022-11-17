namespace WebTemplate.Authentication;

public interface IRoleService
{
    string[] GetRoles();

    bool IsInRole(string roleName);
}
