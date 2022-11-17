using System.ComponentModel.DataAnnotations;
using Namotion.Reflection;
using NJsonSchema.Generation;

namespace WebTemplate.Web.Json;

public class CustomSchemaNameGenerator : DefaultSchemaNameGenerator
{
    public override string Generate(Type type)
    {
        var cachedType = type.ToCachedType();
        var inheritedAttribute = cachedType.GetInheritedAttribute<DisplayAttribute>();
        if (!string.IsNullOrEmpty(inheritedAttribute?.Name))
        {
            return inheritedAttribute.Name;
        }
        return base.Generate(type);
    }
}
