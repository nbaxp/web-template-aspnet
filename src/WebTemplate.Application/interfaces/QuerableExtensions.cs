using System.Linq.Expressions;
using System.Reflection;

namespace WebTemplate.Application.interfaces;

public static class RepositoryExtensions
{
    private static string PROVIDER_NAME = "EntityQueryProvider";
    private static string TYPE_FULL_NAME = "Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions";
    private static string AS_NO_TRACKING_NAME = "AsNoTracking";
    private static string INCLUDE_NAME = "Include";
    private static string THEN_INCLUDE_NAME = "ThenInclude";

    private static MethodInfo[]? Methods;

    public static IQueryable<TEntity> AsNoTracking<TEntity>(
        this IQueryable<TEntity> source)
        where TEntity : class
    {
        if (source.Provider.GetType().Name == PROVIDER_NAME)
        {
            var methodInfo = GetMethods(source.Provider)
                .First(o => o.Name == AS_NO_TRACKING_NAME);
            return source.Provider.CreateQuery<TEntity>(
                          Expression.Call(
                              instance: null,
                              method: methodInfo!.MakeGenericMethod(typeof(TEntity)),
                              arguments: source.Expression));
        }
        return source;
    }

    public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
         this IQueryable<TEntity> source,
         Expression<Func<TEntity, TProperty>> path)
         where TEntity : class
    {
        if (source.Provider.GetType().Name == PROVIDER_NAME)
        {
            var methodInfo = GetMethods(source.Provider)
                .First(o => o.Name == INCLUDE_NAME && o.GetGenericArguments().Length == 2);
            var query = CreateQuery<TEntity>(source, methodInfo!, path, typeof(TEntity), typeof(TProperty));
            return new IncludableQueryable<TEntity, TProperty>(query);
        }
        return new IncludableQueryable<TEntity, TProperty>(source);
    }

    public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
        Expression<Func<TPreviousProperty, TProperty>> path)
        where TEntity : class
    {
        if (source.Provider.GetType().Name == PROVIDER_NAME)
        {
            var type = source.Provider.GetType().Assembly.GetType(TYPE_FULL_NAME);
            var methodInfo = GetMethods(source.Provider)
                .Where(o => o.Name == THEN_INCLUDE_NAME && o.GetGenericArguments().Length == 3)
                .First(o => o.GetParameters()[0].ParameterType.GenericTypeArguments[1].GetGenericTypeDefinition() == typeof(IEnumerable<>));
            var query = CreateQuery<TEntity>(source, methodInfo!, path, typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty));
            return new IncludableQueryable<TEntity, TProperty>(query);
        }
        return new IncludableQueryable<TEntity, TProperty>(source);
    }

    public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableQueryable<TEntity, TPreviousProperty> source,
        Expression<Func<TPreviousProperty, TProperty>> path)
        where TEntity : class
    {
        if (source.Provider.GetType().Name == PROVIDER_NAME)
        {
            var type = source.Provider.GetType().Assembly.GetType(TYPE_FULL_NAME);
            var methodInfo = GetMethods(source.Provider)
                .Where(o => o.Name == THEN_INCLUDE_NAME && o.GetGenericArguments().Length == 3)
                .First(o => o.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);
            var query = CreateQuery<TEntity>(source, methodInfo!, path, typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty));
            return new IncludableQueryable<TEntity, TProperty>(query);
        }
        return new IncludableQueryable<TEntity, TProperty>(source);
    }

    private static MethodInfo[] GetMethods(IQueryProvider provider)
    {
        return Methods == null ? Methods = provider.GetType().Assembly.GetType(TYPE_FULL_NAME)!.GetMethods() : Methods;
    }

    private static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> source, MethodInfo methodInfo, Expression path, params Type[] types)
    {
        return source.Provider.CreateQuery<TEntity>(
            Expression.Call(
                instance: null,
                method: methodInfo.MakeGenericMethod(types),
                arguments: new[] { source.Expression, Expression.Quote(path) }
                )
            );
    }
}
