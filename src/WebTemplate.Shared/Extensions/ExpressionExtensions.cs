using System.Linq.Expressions;

namespace WebTemplate.Shared.Extensions;

public static class ExpressionExtensions
{
    public static string? GetName<T>(this Expression<Func<T, object?>> expression)
    {
        return ((expression.Body as UnaryExpression)?.Operand as MemberExpression)?.Member.Name;
    }
}
