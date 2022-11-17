using Omu.ValueInjecter;

namespace WebTemplate.Shared.Extensions;

public static class ObjectMapperExtensions
{
    public static T To<T>(this object source)
    {
        var target = Activator.CreateInstance<T>();
        target.InjectFrom(source);
        return target!;
    }

    public static T From<T>(this T target, object source)
    {
        target.InjectFrom(source);
        return target;
    }
}
