using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebTemplate.Web.Extensions;

public static class MemberInfoExtensions
{
    public static string GetDisplayName(this MemberInfo memberInfo)
    {
        if (memberInfo is null)
        {
            throw new ArgumentNullException(nameof(memberInfo));
        }
        return memberInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? memberInfo.Name;
    }
}
