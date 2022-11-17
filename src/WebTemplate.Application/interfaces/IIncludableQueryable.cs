namespace WebTemplate.Application.interfaces;

public interface IIncludableQueryable<out TEntity, out TProperty> : IQueryable<TEntity>
{
}
