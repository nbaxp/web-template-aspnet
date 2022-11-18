using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebTemplate.Shared.Extensions;

public static class MemberInfoExtensions
{
    public static string GetDisplayName(this MemberInfo memberInfo)
    {
        return memberInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? memberInfo.Name;
    }
}
