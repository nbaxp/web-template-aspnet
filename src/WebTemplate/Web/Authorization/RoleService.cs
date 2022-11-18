namespace WebTemplate.Authentication;

public class RoleService : IRoleService
{
    private List<string> _roleRepository = new() { "admin1", "user" };

    private string[]? _roles = null;

    public string[] GetRoles()
    {
        if (this._roles == null)
        {
            this._roles = _roleRepository.ToArray();
        }
        return this._roles;
    }

    public bool IsInRole(string roleName)
    {
        return _roleRepository.Any(o => o == roleName);
    }
}
